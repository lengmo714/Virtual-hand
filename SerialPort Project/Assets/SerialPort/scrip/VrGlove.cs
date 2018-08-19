using UnityEngine;
using System.Collections;
using System; 
using System.Threading; 
using System.Collections.Generic; 
using System.ComponentModel; 
using System.IO.Ports; 
using System.Text.RegularExpressions; 
using System.Text; 
using System.Threading;

public class VrGlove : MonoBehaviour
{
    private Transform i_Transform;
    private Transform m_Transform;
    private Transform t_Transform;
    private Transform r_Transform;
    private Transform p_Transform;
	private Transform m1_Transform;
    private Transform W_Transform;
	private GameObject[] ring_Transform;
	SerialPort spstart;
    float rx;
    float ry;  
    float rz;
    float pz;
    float iz;
    float mz;
    float tz;
    float  Mz;
    float My;
    float Mx;
	void Start ()
	{
       
		m1_Transform = gameObject.GetComponent<Transform> ();
		ring_Transform = GameObject.FindGameObjectsWithTag("ring");
        i_Transform = GameObject.Find("R_index_a").GetComponent<Transform>();
        m_Transform = GameObject.Find("R_middle_a").GetComponent<Transform>();
        t_Transform = GameObject.Find("R_thumb_a").GetComponent<Transform>();
        p_Transform = GameObject.Find("R_pinky_a").GetComponent<Transform>();
        W_Transform = GameObject.Find("R_Palm").GetComponent<Transform>();
       for (int i = 0; i < ring_Transform.Length; i++)
        {
            if(ring_Transform[i].gameObject.name.Equals("R_ring_a"))
            {
                r_Transform = ring_Transform[i].GetComponent<Transform>();
                rx = r_Transform.localEulerAngles.x;
                ry = r_Transform.localEulerAngles.y;
                rz = r_Transform.localEulerAngles.z;
            }
        }
        spstart = new SerialPort("COM5", 115200, Parity.None, 8, StopBits.One);

		try
		{
			spstart.Open();
		}
		catch (Exception e)
		{
			Debug.Log(e.ToString());
		}
	}
	

	void Update ()
	{
		Byte[] RxBuffer = new Byte[700];
		Byte[] result1=new Byte[700];
		Byte[] byteTemp=new Byte[700];
		double[] Data = new double[4];
	    int usRxLength=0;
		int Uslength=0;
		
                byte[] start = new byte[1];
                start[0] = 0xcc;
                spstart.Write(start, 0, 1);
				//	tickcount++;
			Uslength=(int)spstart.Read(RxBuffer, usRxLength, 700);
	
		/*	catch (Exception e)
			{
				Debug.Log(e.ToString());

			}
			*/
		
				//Debug.Log(buf[0]);
				//Debug.Log("jieshou");
				//m_Transform.Rotate(Vector3.forward,buf[0]);
				usRxLength+=Uslength;
		      //  Debug.Log (usRxLength);
		while(usRxLength >= 15)
			   {

					RxBuffer.CopyTo(byteTemp, 0);

					if (!((byteTemp[0] == 0x55) & (byteTemp[1] == 0x53)))
					{


				
				for (int i = 1; i < usRxLength; i++) RxBuffer[i - 1] = RxBuffer[i];
				usRxLength--;
				continue;

					}
			{

				Data[0] = BitConverter.ToInt16(byteTemp, 2);
				Data[0] = Data[0] / 32768.0 * 180;           //X

				Data[1] = BitConverter.ToInt16(byteTemp, 4);
				Data[1] = Data[1] / 32768.0 * 180;           //Y

				Data[2] = BitConverter.ToInt16(byteTemp, 6);
				Data[2] = Data[2] / 32768.0 * 180;          //Z
                Mx =(float)(Data[2] + 100);
                My = (float)(Data[1]*2.2);
                Mz =(float)(Data[0]+140);
              // W_Transform.rotation = Quaternion.Euler(Mz,113,202);
           //     W_Transform.localEulerAngles = new Vector3(-36, 110, Mz);
             //  Debug.Log("dATA[2]:" + Data[2]);
              //  W_Transform.localEulerAngles = new Vector3(My,107, 206);
                W_Transform.localEulerAngles = new Vector3(My,Mx, Mz);

                if ((byteTemp[8] == 0xaa) & (byteTemp[14] == 0xbb))
                {
                    result1[8] = byteTemp[8];
                    result1[9] = byteTemp[9];

                    result1[10] = byteTemp[10];
                    result1[11] = byteTemp[11];
                    result1[12] = byteTemp[12];
                    result1[13] = byteTemp[13];
                    result1[14] = byteTemp[14];
                    pz = (result1[9] + 217);
                    rz = (result1[10] + 207);
                    mz = (result1[11] + 217);
                    iz = (result1[12] + 217);
                    tz = (result1[13] - 202);

                    Debug.Log("dATA[2]:" + tz);
                    //  Debug.Log ("小指"+result1 [1]);
                    //  rz = Convert.ToInt16(result1[1]*0.9);
                    //   Thread.Sleep(100);

                    //   p_Transform.rotation = Quaternion.AngleAxis(rz, Vector3.Cross(Vector3.forward,Vector3.up));
                    r_Transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, rz);
                    p_Transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, pz);
                    m_Transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, mz);
                    i_Transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, iz);
                    t_Transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, tz);
                    //    );
                    // Debug.Log("rx:" + rx);
                    //Debug.Log("ry:" + ry);
                    //  Debug.Log("result1[2]:" + result1[2]);

                }

               

			}

			for (int i = 15; i < usRxLength; i++) RxBuffer[i - 15] = RxBuffer[i];
			usRxLength -= 15;
		}
				/*for (int  i=0;i<index_Transform.Length;i++)
				{
					if(index_Transform[i].name.Equals("L_index_c"))
					
						{		
					           // Debug.Log (result1 [5]);
								index_Transform[i].GetComponent<Transform>().Rotate(Vector3.forward,result1[3] );
						}
				}*/
			 }
	

		}


