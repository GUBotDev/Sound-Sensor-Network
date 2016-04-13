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
public class Calculation {
    //from arduino code
    private static float determineAngle(short[] sensors)
    {
        float highVal = 0;
        int highSen = 0;
        int leftHighSen = 0;
        int rightHighSen = 0;

        for (int i = 0; i < 6; i++)
        {
            if (sensors[i] > highVal)
            {
                highVal = sensors[i];
                highSen = i;
            }
        }

        switch (highSen) {
            case 0:
                leftHighSen = 5;
                rightHighSen = 1;
                break;
            case 5:
                leftHighSen = 4;
                rightHighSen = 0;
                break;
            default:
                leftHighSen = highSen - 1;
                rightHighSen = highSen + 1;
                break;
        }

        float pi = (float) Math.PI;
        float sq3 = (float) Math.sqrt(3);

        float s, s1, s2, t, t1, t2, tF;

        s1 = (float) Math.sqrt(sensors[highSen] * sensors[highSen] + sensors[highSen] * sensors[leftHighSen] + sensors[leftHighSen] * sensors[leftHighSen]);//modified sensor 1
        s2 = (float) Math.sqrt(sensors[highSen] * sensors[highSen] + sensors[highSen] * sensors[rightHighSen] + sensors[rightHighSen] * sensors[rightHighSen]);//modified sensor 2

        t1 = (float) Math.atan((sq3 * sensors[leftHighSen]) / (2 * sensors[highSen] + sensors[leftHighSen]));//base angle 1
        t2 = (float) Math.atan((sq3 * sensors[rightHighSen]) / (2 * sensors[highSen] + sensors[rightHighSen]));//base angle 2

        t = (float) Math.atan((s2 * Math.sin(t1 + t2)) / s1 + s2 * Math.cos(t1 + t2));//final mid-angle
        s = (float) Math.sqrt((s1 + s2 * Math.cos(t1 + t2)) * (s1 + s2 * Math.cos(t1 + t2)) + (s2 * Math.sin(t1 + t2)) * (s2 * Math.sin(t1 + t2)));//final sensors magnitude

        tF = highSen * 60 + (t1 - t) * (180 / pi);//final angle

        return tF;
    }

    private static float degToRad(float angle)
    {
        return (float) (Math.PI * angle / 180);
    }

    private static float radToDeg(float angle)
    {
        return (float) (angle * (180.0 / Math.PI));
    }
}
