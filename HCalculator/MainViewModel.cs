using System;
using System.Windows;

namespace HCalculator
{
    public class MainViewModel : BaseViewModel
    {
        #region Привязки
        /// <summary>
        /// Результат ввода и подсчетов котороый отображается на табло
        /// </summary>
        public string Result { get => result; set { result = value; OnPropertyChanged("Result"); } }
        string result = "";

        /// <summary>Первый операнд</summary>
        public string Operand1 { get => operand1; set { operand1 = value; OnPropertyChanged("Operand1"); } }
        string operand1 = "";

        /// <summary>Первый операнд</summary>
        public string Operation { get => operation; set { operation = value; OnPropertyChanged("Operation"); } }
        string operation = "";

        ///// <summary>Второй операнд</summary>
        //public string Operand2 { get => operand2; set { operand2 = value; OnPropertyChanged("Operand2"); } }
        //string operand2 = "";
        //bool useOperand2 = false;

        public bool Advanced { get => advanced; set { advanced = value; OnPropertyChanged("Advanced"); } }
        bool advanced; 
        #endregion

        public MainViewModel()
        {
            SetDigitCommand = new HCommand(SetDigit);
            SetOperationCommand = new HCommand(SetOperation);
            CalculeteCommand = new HCommand(Calculete);
            ClearCommand = new HCommand(Clear);
            OpenAdvancedFuncCommand = new HCommand(OpenAdvancedFunc);
        }

        private void OpenAdvancedFunc(object obj)
        {
            Advanced = !Advanced;
        }

        private void Clear(object obj)
        {
            Operation = Operand1 = Result = "";
            //point = false;
            Advanced = false;
        }

        private void Calculete(object obj)
        {
            if (double.TryParse(Operand1, out double op1) && double.TryParse(Result, out double op2))
            {
                Execute(op1, op2);
                
            }
            else if(double.TryParse(Operand1, out double op))
            {
                Execute(op, 0);
            }
            else if (double.TryParse(Result, out double ope))
            {
                Execute(ope, 0);
            }

        }

        private void Execute(double op1, double op2)
        {
            try
            {
                double result = action(op1, op2);
                Result = result.ToString();
            }
            catch (Exception /*ex*/)
            {
                MessageBox.Show("Произошла ошибка подсчетов!");
            }
            Operand1 = Operation = "";
            Advanced = false;
        }

        delegate double MathAction(double o1, double o2);
        MathAction action;
        private void SetOperation(object obj)
        {
            

            if (obj is string str)
            {
                
                switch (str)
                {
                    case "+": action = new MathAction((x, y) => { return x + y; }); break;
                    case "-": action = new MathAction((x, y) => { return x - y; }); break;
                    case "/": action = new MathAction((x, y) => { return x / y; }); break;
                    case "*": action = new MathAction((x, y) => { return x * y; }); break;
                    
                    case "abs": action = new MathAction((x, y) => { return Math.Abs(x); }); Calculete(null); return;
                    case "sin": action = new MathAction((x, y) => { return Math.Sin(x); }); Calculete(null); return;
                    case "cos": action = new MathAction((x, y) => { return Math.Cos(x); }); Calculete(null); return;
                    case "tan": action = new MathAction((x, y) => { return Math.Tan(x); }); Calculete(null); return;
                    
                    case "rnd": action = new MathAction((x, y) => { return Math.Round(x,0); }); Calculete(null); return;
                    case "rnd0": action = new MathAction((x, y) => { return Math.Round(x,1); }); Calculete(null); return;
                    case "rnd00": action = new MathAction((x, y) => { return Math.Round(x,2); }); Calculete(null); return;
                    case "rnd000": action = new MathAction((x, y) => { return Math.Round(x,3); }); Calculete(null); return; 
                    
                    case "x2": action = new MathAction((x, y) => { return Math.Pow(x,2); }); Calculete(null); return;
                    case "xp": action = new MathAction((x, y) => { return Math.Pow(x,y); }); break;
                    case "sqrt": action = new MathAction((x, y) => { return Math.Sqrt(x); }); Calculete(null); return;
                    case "inv":
                        if (double.TryParse(Result, out double op))
                        {
                            Result = (op * (-1)).ToString();
                            return;

                        }
                            break;

                    //case "*": action = new MathAction((x, y) => { return x / y; }); break;


                    default: MessageBox.Show("Программист! Новая операция! Доработай 'SetOperation'"); break;
                }
                Operation = str;
                Operand1 = Result;
                //point = false;
                Result = "";
                Advanced = false;
            }
        }

        //bool point = false;
        

        private void SetDigit(object obj)
        {// Ввести опернад и вывести на табло
            // проверить можно ли операнд распарсить
            //MessageBox.Show(obj.GetType().Name);
            if (obj is string str)
            {
                if (str == ","&& !Result.Contains(str))
                {
                    Result += str;
                    //point = true;
                }
                else
                    if (double.TryParse(Result + str, out double operand))
                {
                    Result = operand.ToString();
                }
                else
                {
                    MessageBox.Show("Не возможная операция!");
                }
                Advanced = false;
            }
        }
        public HCommand SetDigitCommand { get; set; }
        public HCommand SetOperationCommand { get; set; }
        public HCommand CalculeteCommand { get; set; }
        public HCommand ClearCommand { get; set; }
        public HCommand OpenAdvancedFuncCommand { get; set; }

    }
}
