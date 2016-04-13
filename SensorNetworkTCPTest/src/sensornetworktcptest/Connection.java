/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package sensornetworktcptest;

import java.io.*;
import java.net.*;
import java.util.ArrayList;
import java.util.Date;
import java.text.DateFormat;
import java.text.SimpleDateFormat;

/**
 *
 * @author Rollie
 */
public class Connection {
    private static String ipString = "10.1.25.248";
    private static int nodeNum = 1, x = 0, y = 0;
    private static InetAddress ip;
    private static Socket commandR;
    private static Socket commandW;
    private static Socket audio;
    private static boolean isWritingAudio = false;
    private static byte[] audioSend;
    private static String audioInformation = "";
    private static ArrayList<String> commandSend = new ArrayList<String>();
    
    public static void addCommand(String command){
        commandSend.add(command);
    }
    
    public static boolean isWritingAudio(){
        return isWritingAudio;
    }
    
    public static void setWritingAudio(boolean isWritingAudio){
        Connection.isWritingAudio = isWritingAudio;
    }
    
    public static void setAudioInformation(String info){
        audioInformation = info;
    }
    
    public static void setAudio(byte[] data){
        audioSend = data;
    }
    
    public void commandWriteThread(){
        try {
            Thread.sleep(1000);
            
            Runnable run;
            run = () -> {
                while(true){
                    try{
                        commandW = new Socket(ip, 10001);
                        DataOutputStream audioWriter = new DataOutputStream(commandW.getOutputStream());   

                        while(!commandW.isConnected()){
                            Thread.sleep(1000);
                            
                            System.out.println("Not connected");
                        }
                        
                        try
                        {
                            System.out.println("Write Command Thread Started");
                            DateFormat dF = new SimpleDateFormat("yyyy-MM-dd_HH-mm-ss.SSSS");
                            //commandSend.add("Event " + dF.format(new Date()));

                            while (true)
                            {
                                //commandSend.add("Event " + dF.format(new Date()));
                                
                                if (commandSend.toArray().length > 0){
                                    audioWriter.writeBytes((String)commandSend.toArray()[0] + "\n");
                                    audioWriter.flush();

                                    System.out.println("Sent: " + (String)commandSend.toArray()[0]);

                                    commandSend.remove(0);
                                }
                                else{
                                    Thread.sleep(1);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //System.out.println(ex);
                        }

                        commandW.close();
                        Thread.sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        //System.out.println(ex);
                    }
                }
            };

            new Thread(run).start();
        }
        catch (Exception ex){
            
        }
    }
    
    public void commandReadThread(){
        try{
            ip = InetAddress.getByName(ipString);
            
            Runnable run;
            run = () -> {
                while(true){
                    try{
                        commandR = new Socket(ip, 10000);
                        BufferedReader commandReader = new BufferedReader(new InputStreamReader(commandR.getInputStream()));

                        try
                        {
                            System.out.println("Read Command Thread Started");

                            while (true)
                            {
                                DateFormat dF = new SimpleDateFormat("yyyy-MM-dd_HH-mm-ss.SSSS");
                                String line = commandReader.readLine();
                                String[] split = line.split(" ");

                                System.out.println("Line: " + line);
                                
                                //System.out.println(split[0]);
                                
                                if ("Request".equals(split[0])){
                                    Date date = dF.parse(split[1]);

                                    short[] data = Audio.getArrayAtTime(date);

                                    if (data.length > 1){
                                        commandSend.add("Node " + nodeNum + " " + x + " " + y + " " + data[0] + " " + data[1] + " " + data[2] + " " + data[3] + " " + data[4] + " " + data[5] + " " + dF.format(date));
                                    }
                                    else{
                                        commandSend.add("Node " + nodeNum + " " + -1 + " " + -1 + " " + -1 + " " + -1 + " " + -1 + " " + -1 + " " + x + " " + y + " " + dF.format(date));
                                    }
                                }
                                else if ("RequestAudio".equals(split[0])){
                                    Date date = dF.parse(split[1]);
                                    int seconds = Integer.parseInt(split[2]);

                                    Audio.sendAudio(seconds);
                                }
                            }
                        }
                        catch (Exception ex){
                            System.out.println("One: " + ex);
                        }

                        commandR.close();
                        Thread.sleep(1000);
                    }
                    catch (Exception ex){
                        //System.out.println("Two: " + ex);
                    }
                }
            };

            new Thread(run).start();
        }
        catch (Exception ex){
            //System.out.println("Three: " + ex);
        }
    }
    
    public void audioThread(){
        try {
            Thread.sleep(1000);
            
            Runnable run;
            run = () -> {
                while(true){
                    try{
                        audio = new Socket(ip, 9999);
                        DataOutputStream audioWriter = new DataOutputStream(audio.getOutputStream());   

                        try
                        {
                            System.out.println("Audio Thread Started");

                            while (true)
                            {
                                if (isWritingAudio){
                                    audioWriter.writeBytes(audioInformation);

                                    audioWriter.write(audioSend, 0, audioSend.length);

                                    isWritingAudio = false;
                                    
                                    System.out.println("Sending Audio");
                                }
                                else{
                                    Thread.sleep(10);
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        audio.close();
                        Thread.sleep(1000);
                    }
                    catch (Exception ex){

                    }
                }
            };

            new Thread(run).start();
        }
        catch (Exception ex){
            
        }
    }
}