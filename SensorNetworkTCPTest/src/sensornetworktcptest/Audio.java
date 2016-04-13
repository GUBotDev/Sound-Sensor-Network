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
import java.util.Collections;
import java.util.concurrent.TimeUnit;//.sleep(long timeout)
import java.util.Comparator;

/**
 *
 * @author Rollie
 */
public class Audio {
    //read audio and store data
    private static Map<Date, short[]> audioMap = new HashMap<>();
    private static short tolerance = 20000;//32768
    private static int trigSensor;
    
    public static short getTolerance(){
        return tolerance;
    }
    public static void setTolerance(short tolerance){
        Audio.tolerance = tolerance;
    }
    
    public static void addToMap(Date date, short[] audio){
        synchronized(Audio.audioMap){
            audioMap.put(date, audio);
        }
        
        //audioMap.put(date, audio);
    }
    
    public static Map<Date, short[]> returnMap(){
        return audioMap;
    }
    
    public void listen(){
        try{
            Runnable run;
            run = () -> {
                try{
                    while (true)
                    {
                        short[] tempAudio = new short[6];
                       
                        tempAudio[0] = 0;
                        tempAudio[1] = 0;
                        tempAudio[2] = 0;
                        tempAudio[3] = 0;
                        tempAudio[4] = 0;
                        tempAudio[5] = 0;
                       
                        Date date = new Date();
                       
                        addToMap(date, tempAudio);
                       
                        interpret(tempAudio, date);
                        
                        /*
                        MCP3008 200KSPS
                        -may as well use a damn arduino as an adc
                        
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
    
    static Comparator<Date> comparator = (Date o1, Date o2) -> o1.compareTo(o2);
    
    public static short[] getArrayAtTime(Date date){
        short[] data = new short[0];
        
        try
        {
            synchronized(Audio.audioMap){
                Map<Date, short[]> tempMap = new HashMap<>(Audio.returnMap());
                ArrayList<Date> dateList = new ArrayList<>(tempMap.keySet());

                if (dateList.toArray().length > 0 && date.after((Date)dateList.toArray()[0])){
                    int index = Collections.binarySearch(dateList, date, comparator);

                    //System.out.println("AudioOut: " + index + " " + dateList.toArray().length + " " + (dateList.toArray().length + index + 1));

                    Date knownDate = (Date)dateList.toArray()[dateList.toArray().length + index + 1];

                    data = tempMap.get(knownDate);
                    
                    //System.out.println(data[0]);
                }
            }
        }
        catch(Exception ex)
        {
            System.out.println("Get Array At Time: " + ex);
        }
                 
        return data;             
    }
    
    public static short[] getAudio(Date date1, Date date2){
        short[] data = new short[0];
        
        try
        {
            synchronized(Audio.audioMap){
                Map<Date, short[]> tempMap = new HashMap<>(Audio.returnMap());
                ArrayList<Date> dateList = new ArrayList<>(tempMap.keySet());

                int index1 = Collections.binarySearch(dateList, date1, comparator);
                int index2 = Collections.binarySearch(dateList, date2, comparator);

                data = new short[index2 - index1];
                int iterator = 0;

                Date[] dates = (Date[])dateList.toArray();

                for (int i = index1; i <= index2; i++){
                    Date knownDate = dates[i];
                    data[iterator] = tempMap.get(knownDate)[trigSensor];

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
    
    public static void sendAudio(int timeSeconds){
        Runnable run;
        run = () -> {
            try{
                synchronized(Audio.audioMap){
                    Thread.sleep(5000);
                    Map<Date, short[]> tempMap = new HashMap<>(Audio.returnMap());

                    if (Connection.isWritingAudio()){
                        int tempSize = 44100 * 10;
                        short[] temp = new short[tempSize];
                        byte[] tempB = new byte[tempSize * 2];

                        if (tempMap.size() < tempSize){
                            tempSize = tempMap.size();
                        }

                        for (int i = tempMap.size(); i > tempMap.size() - tempSize; i--){
                            temp[i] = ((short[])(tempMap.values().toArray()[i]))[trigSensor];
                        }

                        for (int i = 0; i < tempB.length; i += 2){
                            byte[] tempBytes = returnBytes(temp[i / 2]);

                            //short to byte
                            tempB[i] = tempBytes[0];
                            tempB[i + 1] = tempBytes[1];
                        }

                        DateFormat dF = new SimpleDateFormat("yyyy-MM-dd_HH-mm-ss.SSSS");

                        Connection.setAudioInformation("Audio " + dF.format((Date)(tempMap.keySet().toArray()[tempMap.size() - tempSize])) + " " + tempSize);
                        Connection.setAudio(tempB);
                        Connection.setWritingAudio(true);
                    }
                }
                //send temp
            }
            catch (Exception ex)
            {
                System.out.println("Send Audio: " + ex);
            }
        };
                    
        new Thread(run).start();
        //writeline date, length bytes, then send bytes
    }
    
    private static byte[] returnBytes(short input)
    {
        String inputShort = String.format("%016d", Integer.parseInt(Integer.toBinaryString(input)));

        String one = inputShort.substring(0, 8);
        String two = inputShort.substring(8, 8);
        
        byte[] tempByte = new byte[2];

        tempByte[0] = Byte.parseByte(one, 2);
        tempByte[1] = Byte.parseByte(two, 2);
        
        return tempByte;
    }

}
