using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class ByteQueue
{
    private byte[] buffer = null;
    private int head = 0;
    private int tail = 0;
    private int capacity = 0;

    public ByteQueue(int cap)
    {
        if (cap > 512)
        {
            capacity = 1024;
        }
        capacity = 512;
        buffer = new byte[capacity];
    }

    public void Enqueue(byte[] b)
    {
        if (b == null || b.Length == 0)
        {
            return;
        }
        if (capacity - Math.Abs(tail - head) < b.Length)
        {
            Extand();
        }
        if (tail > head)
        {
            if (capacity - tail > b.Length)
            {
                Array.Copy(b, 0, buffer, tail, b.Length);
                tail += b.Length;
            }
        }
    }

    public byte Dequeue()
    {
        if (head == tail)
        {
            return 0;
        }
        return buffer[head++];
    }

    public byte[] DequeueLine()
    {
        if (head == tail)
        {
            return null;
        }
        byte[] b = ASCIIEncoding.ASCII.GetBytes("\n");
        int idx = head;
        if (head < tail)
        {
            while (buffer[idx] != b[0])
            {
                if (idx < tail)
                {
                    idx++;
                }
                else
                {
                    return null;
                }
            }
            Debug.Assert(idx > head);
            byte[] r = new byte[idx - head + 1];
            Array.Copy(buffer, head, r, 0, idx - head + 1);
            head = idx + 1;
            return r;
        }
        else
        {
            bool flag = false;
            while (buffer[idx] != b[0])
            {
                if (idx < capacity)
                {
                    idx++;
                }
                else
                {
                    flag = true;
                    idx = 0;
                }
                if (idx == tail)
                {
                    return null;
                }
            }
            if (flag)
            {
                byte[] r = new byte[head - idx];
                Array.Copy(buffer, head, r, 0, capacity - head);
                Array.Copy(buffer, 0, r, capacity - head, idx - 0 + 1);
                head = idx + 1;
                return r;
            }
            else
            {
                byte[] r = new byte[idx - head + 1];
                Array.Copy(buffer, head, r, 0, idx - head + 1);
                head = idx + 1;
                return r;
            }
        }
    }

    private void Extand()
    {
        Debug.Assert(false, "now ");
    }

}
