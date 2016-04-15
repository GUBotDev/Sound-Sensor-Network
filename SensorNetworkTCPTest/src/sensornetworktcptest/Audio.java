/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package sensornetworktcptest;

import java.util.HashMap;
import java.util.Map;
import java.util.Date;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Calendar;
import java.util.Collections;
import java.util.concurrent.TimeUnit;//.sleep(long timeout)
import java.util.Comparator;

/**
 *
 * @author Rollie
 */
public class Audio {
    //read audio and store data
    private static short tolerance = 20000;//32768
    private static int trigSensor = 0;
    
    public static short getTolerance(){
        return tolerance;
    }
    public static void setTolerance(short tolerance){
        Audio.tolerance = tolerance;
    }
    
    
    private static ArrayList<Date> dates = new ArrayList<>();
    private static ArrayList<short[]> audio = new ArrayList<>();
    
    public void listen(){
        try{
            Runnable run;
            run = () -> {
                try{
                    while (true)
                    {
                        short[] tempAudio = new short[6];
                       
                        tempAudio[0] = (short)(Math.random() * 20000);
                        tempAudio[1] = (short)(Math.random() * 20000);
                        tempAudio[2] = (short)(Math.random() * 20000);
                        tempAudio[3] = (short)(Math.random() * 20000);
                        tempAudio[4] = (short)(Math.random() * 20000);
                        tempAudio[5] = (short)(Math.random() * 20000);
                       
                        Date date = new Date();
                       
                        dates.add(date);
                        audio.add(tempAudio);
                        
                        //addToMap(date, tempAudio);
                       
                        interpret(tempAudio, date);
                        
                        TimeUnit.NANOSECONDS.sleep(22676);//22676, (int)((1 / (44100)) * 1000000000)
                            
                        /*
                        MCP3008 200KSPS
                        -may as well use a damn arduino as an adc
                        -
                        
                        long average = 0;
                        
                        for (int i = 0; i < 100; i++){
                            long start, elapsedTime;
                            start = System.nanoTime();
                            
                            TimeUnit.NANOSECONDS.sleep(10);//22676, (int)((1 / (44100)) * 1000000000)
                            //interpret(tempAudio, date);
                            
                            elapsedTime = System.nanoTime() - start;
                            
                            average = (average + elapsedTime) / 2;
                        }
                        
                        System.out.println("Average: " + average);
                        */
                       
                        //TimeUnit.NANOSECONDS.sleep((int)((1 / (44100)) * 1000000000));
                        //System.out.println("Audio Time: " + elapsedTime);
                        //Thread.sleep(1000);
                    }
                }
                catch (Exception ex){
                    System.out.println("Listen: " + ex);
                }
            };
                    
            new Thread(run).start();
        }
        catch (Exception ex){
            
        }
    }
    
    public static void interpret(short[] sensors, Date date){
        //float angle = determineAngle(sensors);
        
        short maxValue = 0;
        int maxSens = 0;
        
        for (int i = 0; i < sensors.length; i++){
            if (sensors[i] > maxValue){
                maxValue = sensors[i];
                maxSens = i;
            }
        }
        
        trigSensor = maxSens;
        
        if (Math.abs(maxValue) > tolerance){
            DateFormat dF = new SimpleDateFormat("yyyy-MM-dd_HH-mm-ss.SSSS");
            
            Connection.addCommand("Event " + dF.format(date));
        }
    }
    
    static Comparator<Date> comparator = (Date d1, Date d2) -> d1.compareTo(d2);
    
    public static short[] getArrayAtTime(Date date){
        short[] data = new short[0];
        
        try
        {
            synchronized(Audio.dates){
                ArrayList<Date> dateList = new ArrayList<>(dates);

                if (dateList.toArray().length > 0 && date.after((Date)dateList.toArray()[0])){
                    int index = binarySearch(date);;

                    data = (short[])audio.toArray()[index];
                }
            }
        }
        catch(Exception ex)
        {
            System.out.println("Get Array At Time: " + ex);
        }
                 
        return data;             
    }
    
    public static int binarySearch(Date date){
        int centerKey = 0;
        
        try{
            ArrayList<Date> dates = new ArrayList<>(Audio.dates);
            Date[] dateArr = Arrays.copyOf(dates.toArray(), dates.toArray().length, Date[].class);
            
            //Date[] dateArr = (Date[])dates.toArray();
            Date startDate = dateArr[0];
            Date endDate = dateArr[dateArr.length - 1];


            int lowKey = 0;
            int highKey = dateArr.length - 1;
            int iterator = 0;
            
            //start < end
            while(endDate.equals(startDate) || startDate.before(endDate)){
                centerKey = lowKey + (highKey - lowKey) / 2;
                
                if (date.equals(dateArr[centerKey])){
                    break;
                }
                else if (dateArr[centerKey].before(date)){
                    lowKey = centerKey + 1;
                    startDate = dateArr[lowKey];
                }
                else {
                    highKey = centerKey - 1;
                    startDate = dateArr[highKey];
                }
                
                if (iterator > 1000){
                    break;
                }
                else{
                    iterator++;
                }
            }
        }
        catch(Exception ex){
            System.out.println("Binary Search: " + ex);
        }
        
        return centerKey;
    }
    
    public static short[] getAudio(Date date1, Date date2){
        short[] data = new short[0];
        
        try
        {
            synchronized(Audio.audio){
                //Map<Date, short[]> tempMap = new HashMap<>(Audio.returnMap());
                //ArrayList<Date> dateList = new ArrayList<>(tempMap.keySet());

                int index1, index2;
                
                if (date1 == (Date)dates.toArray()[0]){
                    index1 = 0;
                    index2 = binarySearch(date2);
                }
                else{
                    index1 = binarySearch(date1);
                    index2 = binarySearch(date2);
                }
                
                data = new short[index2 - index1 + 1];
                int iterator = 0;

                for (int i = index1; i <= index2; i++){
                    data[iterator] = ((short [])Audio.audio.toArray()[i])[trigSensor];

                    iterator++;
                }
            }
        }
        catch (Exception ex)
        {
            System.out.println("Get Audio: " + ex);
        }
        
        return data;
    }
    
    public static void sendAudio(Date date, int timeSeconds){
        Runnable run;
        run = () -> {
            try{
                //if the time is requested from the future, it will wait
                if (!new Date().after(new Date(date.getTime() + ((timeSeconds * 1000) / 2)))){
                    long temp = date.getTime() - new Date().getTime() + ((timeSeconds * 1000) / 2);
                    
                    if (temp < 0){
                        temp = 0;
                    }
                    
                    Thread.sleep(temp);
                }
                
                synchronized(Audio.audio){
                    Date startDate = new Date(date.getTime() - (timeSeconds * 1000) / 2);
                    Date endDate = new Date(date.getTime() + (timeSeconds * 1000) / 2);
                    
                    if (startDate.before((Date)dates.toArray()[0])){
                        //System.out.println("Date shift: " + date1 + " to " + (Date)dates.toArray()[0] + " | " + date2);
                        startDate = (Date)dates.toArray()[0];
                    }
                    
                    short[] audio = getAudio(startDate, endDate);
                    long actualTime = (endDate.getTime() - startDate.getTime()) / 1000;
                    
                    long frequency = audio.length / actualTime;
                    byte[] tempB = new byte[audio.length * 2];
                    
                    for (int i = 0; i < tempB.length; i += 2){
                        byte[] tempBytes = returnBytes(audio[i / 2]);

                        //short to bytes
                        tempB[i] = tempBytes[0];
                        tempB[i + 1] = tempBytes[1];
                    }

                    DateFormat dF = new SimpleDateFormat("yyyy-MM-dd_HH-mm-ss.SSSS");

                    Connection.setAudioInformation("Audio " + dF.format(startDate) + " " + audio.length + " " + frequency);
                    Connection.setAudio(tempB);
                    Connection.setWritingAudio(true);
                }
            }
            catch (Exception ex)
            {
                System.out.println("Send Audio: " + ex);
            }
        };
                    
        new Thread(run).start();
    }
    
    private static byte[] returnBytes(short input)
    {
        byte[] tempByte = new byte[2];
        
        try
        {
            String inputShort = String.format("%16s", Integer.toBinaryString(input)).replace(' ', '0');
            
            String one = inputShort.substring(1, 8);
            String two = inputShort.substring(9, 16);
            
            tempByte[0] = Byte.parseByte(one, 2);
            tempByte[1] = Byte.parseByte(two, 2);
        }
        catch (Exception ex)
        {
            System.out.println("ReturnBytes: " + ex);
        }
        
        return tempByte;
    }

}
