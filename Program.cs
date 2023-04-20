using isRock.Core.AOP;
using System;
using System.Reflection;

namespace testAOP
{
    class Program
    {
        static void Main(string[] args)
        {
            //建立BMIProcessor物件
            IBMIProcessor BMI = PolicyInjection.Create<IBMIProcessor>(new BMIProcessor());
            BMI.Height = 170;
            BMI.Weight = 70;
            //計算BMI
            var ret = BMI.Calculate();
            //顯示
            Console.WriteLine($"BMI : {ret}");

            Console.WriteLine("Press any key for continuing...");
            Console.ReadKey();
        }
    }

public interface IBMIProcessor
{
    decimal BMI { get; }
    int Height { get; set; }
    int Weight { get; set; }

    decimal Calculate();
}

public class BMIProcessor : IBMIProcessor
    {
        public int Weight { get; set; }
        public int Height { get; set; }
        public Decimal BMI
        {
            get
            {
                return Calculate();
            }
        }


        //計算BMI
        [Logging(LogFileName = "log.txt")]
        public Decimal Calculate()
        {
            Decimal result = 0;

            Decimal height = (Decimal)Height / 100;
            result = Weight / (height * height);

            return result;
        }
    }

    public class Logging : PolicyInjectionAttributeBase
    {
        //指定Log File Name
        public string LogFileName { get; set; }
        //override AfterInvoke方法
        public override void AfterInvoke(object sender, PolicyInjectionAttributeEventArgs e)
        {
            var msg = $"\r\n Method '{((MethodInfo)sender).Name}' has been called - {DateTime.Now.ToString()} ";
            SaveLog(msg);
        }
        //寫入Log
        private void SaveLog(string msg)
        {
            if (System.IO.File.Exists(LogFileName))
            {
                System.IO.File.AppendAllText(LogFileName, msg);
            }
            else
            {
                System.IO.File.WriteAllText(LogFileName, msg);
            }
        }
    }
}
