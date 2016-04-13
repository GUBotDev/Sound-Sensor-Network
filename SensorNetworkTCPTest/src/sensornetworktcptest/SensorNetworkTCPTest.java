/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package sensornetworktcptest;

/**
 *
 * @author Rollie
 */
public class SensorNetworkTCPTest {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        try{
            Connection conn = new Connection();
            Audio audio = new Audio();
            
            conn.commandReadThread();
            conn.commandWriteThread();
            conn.audioThread();
            audio.listen();
        }
        catch (Exception ex)
        {
            
        }
    }
    
}
