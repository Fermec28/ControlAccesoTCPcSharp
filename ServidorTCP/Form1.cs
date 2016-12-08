using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

//Librerias necesaria para la comunicacion por sockets
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServidorTCP
{
    public partial class Form1 : Form
    {
        IPEndPoint ipEnd = new IPEndPoint(IPAddress.Any, 1234);
        //Declaracion de un socket servidor con las siguientes caracteristicas
        //  InterNetwork    ->  IPV4
        //  Stream          ->  Transmision bidireccional de bytes
        //  TCP             ->  Transsmision Contron Protocol
        Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //Declaracion Socket que se comunicara con el cliente        
        Byte[] Data = new Byte[224];
        private Socket clients;
        Thread workerThread;
        private bool acttivarTcpIp = false;
        private bool activarTime = false;
        int tiempo = 0;
        ManejadorClientes ManejadordeClientes = new ManejadorClientes(2);

        /*Decalaracion de Timers*/

       
    
        public Form1()
        {
            InitializeComponent();
            //Enlazar el servidor con el punto de conexion
            sock.Bind(ipEnd);
            //Definir que el socket va a escuchar a un maximo de 10 clientes simultaneos
            sock.Listen(1);//solo un nano va a acceder a ese servidor
            DateTime date1 = new DateTime(1472850933666564);
            Console.WriteLine(date1.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"));
            Console.WriteLine(DateTime.Now.Ticks);
            Actualizar_Puertos();
            serialPort1.DataReceived += DataReceived;


        }

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
           
        }

        private void Escucharclientes()
        {
            while (acttivarTcpIp)
            {
                try
                {
                    
                    clients = sock.Accept();
                    Console.WriteLine("Empezar Timer");
                    
                }
                catch { }
            }
            Console.Write("ultima vez");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Conectado";
            label1.BackColor = Color.Green;
            acttivarTcpIp = true;
            if (!workerThread.IsAlive)
            {
                workerThread.Start();
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {

                if (clients.Connected)
                {
                    //Preguntar cuantos datos hay para leer
                    if (clients.Available > 0)
                    {
                        clients.Receive(Data);
                        Clientes.Text += "Valor: " + Encoding.ASCII.GetString(Data) + "\n";
                        if (ManejadordeClientes.IsActivo())
                        {
                            ManejadordeClientes.InsertCliente(Encoding.ASCII.GetString(Data));
                        }
                        else
                        {
                            Clientes.Text += "\n Clientes ya registrados";
                        }

                    }
                    try
                    {
                        clients.Send(Data);
                    }
                    catch
                    {
                        clients.Close();

                    }
                }
            }
            catch { }




            if (!ManejadordeClientes.IsActivo()) // si ya estan los clientes ocupados pregunta
            {
                Console.WriteLine("Manejador de clientes no activo");

                if (!activarTime && ManejadordeClientes.IsDiferent())  // el timer no esta activo y los clientes son diferentes
                {
                    //envia el serial 
                    enviarDato();
                    Console.WriteLine("Activar Timer");
                    timer2.Start();
                    activarTime = true;//activa el timer
                }

                else if (!ManejadordeClientes.IsDiferent())// si los clientes son iguales  no activa el timer y borra los registros de los clientes
                {

                    Clientes.Text += "\n ------ clientes iguales intente de nuevo";
                    ManejadordeClientes.ClearClientes();
                }
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            workerThread = new Thread(Escucharclientes);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            sock.Close();
            timer1.Stop();
            acttivarTcpIp = false;
            workerThread.Abort();
            serialPort1.Close();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (activarTime && tiempo < 200) { tiempo++; Console.WriteLine("\n el valor del timer es:" + tiempo); }
            else if (tiempo == 200) { activarTime = false; ManejadordeClientes.ClearClientes(); tiempo = 0; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Actualizar_Puertos();
        }

        private void Actualizar_Puertos() {

            string[] ports = SerialPort.GetPortNames();
            comboBox1.Items.Clear();
            foreach( string puerto in ports) {

                comboBox1.Items.Add(puerto);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = comboBox1.Text;
            serialPort1.BaudRate = 9600;
            try
            {
                if (!serialPort1.IsOpen) {
                    serialPort1.Open();
                }

                             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void enviarDato() {

            byte[] cash = new byte[1];
            cash[0] = 99;
            this.serialPort1.Write(cash, 0, cash.Length);
        }
    }
}
