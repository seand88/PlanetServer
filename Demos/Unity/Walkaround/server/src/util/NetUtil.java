package util;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class NetUtil
{
    public static List<Integer> fromIntArray(int[] array)
    {
        List<Integer> l = new ArrayList<Integer>(array.length);
        for (int s : array)
        {
            l.add(s);
        }
        return (l);
    }

    public static List<String> fromStringArray(String[] array)
    {
        List<String> l = new ArrayList<String>(array.length);
        l.addAll(Arrays.asList(array));
        
        return (l);
    }

    public static int[] toIntArray(List<Integer> list)
    {
        try
        {
            int[] ret = new int[list.size()];

            for (int i = 0; i < ret.length; i++)
            {
                ret[i] = list.get(i).intValue();
            }

            return ret;
        }
        catch (NullPointerException e)
        {
            return null;
        }
    }

    public static float[] toFloatArray(List<Float> list)
    {
        try
        {
            float[] ret = new float[list.size()];

            for (int i = 0; i < ret.length; i++)
            {
                ret[i] = list.get(i).floatValue();
            }

            return ret;
        }
        catch (NullPointerException e)
        {
            return null;
        }
    }

    public static boolean[] toBooleanArray(List<Boolean> list)
    {
        try
        {
            boolean[] ret = new boolean[list.size()];

            for (int i = 0; i < ret.length; i++)
            {
                ret[i] = list.get(i).booleanValue();
            }

            return ret;
        }
        catch (NullPointerException e)
        {
            return null;
        }
    }

    public static String[] toStringArray(List<String> list)
    {
        try
        {
            String[] ret = new String[list.size()];

            for (int i = 0; i < ret.length; i++)
            {
                ret[i] = list.get(i);
            }

            return ret;
        }
        catch (NullPointerException e)
        {
            return null;
        }
    }

    public static double[] toDoubleArray(List<Double> list)
    {
        try
        {
            double[] ret = new double[list.size()];

            for (int i = 0; i < ret.length; i++)
            {
                ret[i] = list.get(i);
            }

            return ret;
        }
        catch (NullPointerException e)
        {
            return null;
        }
    }
}