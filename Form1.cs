using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Omni
{
    public partial class Form1 : Form
    {
        //это глобальные переменные на форме
        float courseOld;    //предыдущий курс (увязан с шагом step. То есть при колебании курса менее шага это значение не изменится, а при 0,1 - изменится)
        float courseNew;    //новый курс
        float step;         //значение критического шага (у тебя это 0,1)

        float ownBtc;       //сколько имеем битков
        float ownOmni;      //сколько у нас омни
        
        public Form1()
        {            
            InitializeComponent();
            //задаем стартовые значения
            button1.Visible = false;             //это кнопка сбрасывает значение курса до 1. пока делаем невидимой
            courseOld = (float)coursValue1.Value; //начальные значения курсов
            courseNew = (float)coursValue1.Value;
            ownOmni = 10;                        //чем располагаем в начале
            ownBtc = 0;
            BankBtc.Text = "BTC: " + ownBtc;     //Выведим то, чем располагаем в label
            bankOmni.Text = "Omni " + ownOmni;
            step = (float)0.1;                   //задаем величину критичности шага в 0,1
            coursValue1.Increment=0.05M;          //задаем шаг изменения курса (при нажатии на бегунок значение курса увеличиватся/уменьшается на это значение)
        }

        //ставим обработчик на событие изменения значения формы курса
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            //если курс дошел до 2, то сделать неактивным и сделать видимой кнопку сброса уровня курса до 1Omni=1Btc.
            if (coursValue1.Value >= 2) {
                coursValue1.Enabled = false;
                button1.Visible = true;
            }

            //обновляем значение нового значения курса, ведь в форме поменялось значение
            //значение старое обновляем в том случае, если дельта между ними будет >= 0.1
            courseNew =  (float)coursValue1.Value; 


            //смотрим дельту между старым и новым значением курса. Round для более правильного округления.
            float dif = (float)Math.Round((float)((courseNew - courseOld)), 2);

            //если dif положительная, то курс Omni к битку растет и это первый кейс (продажа omni). Но только в том случае если дельта превышает или равно заявленным 0,1
            if (dif >= 0)
            {
                if ((courseOld != courseNew) & (dif >= step))
                {
                    if (courseOld < courseNew)
                    {
                        //MessageBox.Show("Продавать 0,1 омни");
                        //функция (описана ниже) по продаже OMNI                         
                        SellMyOmni();
                    }
                    //дельта превышает значение 0.1 => теперь в старое значение записывается текущее новое значение.
                    courseOld = courseNew;

                }
                //отображаем для наглядности текщие значения courseOld и courseNew
                label5.Text = "Предыдущее критическое значение курса " + courseOld.ToString() + ".";
                label6.Text = "Текущее значение курса " + courseNew.ToString() + ".";
            }
            //если dif отрицательная, то курс Битка растет по отношнию к Omni и это второй кейс (продажа битков)
            else
            {
                if ((courseOld != courseNew) & (Math.Abs(dif) >= step))
                {
                    if (courseOld > courseNew)
                    {
                        //MessageBox.Show("Продавать 1 биток");
                        //Функция продажи битков
                        SellMyBtc();
                    }

                    //отображаем для наглядности текщие значения courseOld и courseNew
                    courseOld = courseNew;
                }
                //отображаем для наглядности текщие значения courseOld и courseNew
                label5.Text = "Предыдущее критическое значение курса " + courseOld.ToString() + ".";
                label6.Text = "Текущее значение курса " + courseNew.ToString() + ".";
            }
        }

        //тут логика продажи Оmni при увеличении его курса
        void SellMyOmni()
        {
            //есть омни - есть торговля)
            if (ownOmni > 0)
            {
                //продаем 1 омни
                ownOmni = ownOmni - 1;
                //Увеличиваем значение наших битков на 1 омни (по курсу битка)
                ownBtc += (float)coursValue1.Value;
                //Отобразить на форме результат операции
                BankBtc.Text = "BTC: " + ownBtc;
                bankOmni.Text = "Omni " + ownOmni;
            }
            else
                MessageBox.Show("Omni кончились!");
        }

        //логика продажи битков при уменьшении его курса
        void SellMyBtc()
        {
            //Есть битки - есть продажи
            if (ownBtc > 0)
            {
                //Продаем битков по цене 1 омни
                ownBtc = ownBtc - (float)coursValue1.Value;
                //Покупаем 1 омни
                ownOmni = ownOmni + 1;
                
                //Отобразить на форме результат операции
                BankBtc.Text = "BTC: " + ownBtc;
                bankOmni.Text = "Omni " + ownOmni;
            }
            else
                MessageBox.Show("Btc кончились!");
        }

        //обработчик кнопочки, обнуляющей значение курсов. 
        private void button1_Click(object sender, EventArgs e)
        {
            courseOld = 0;
            courseNew = 0;
            coursValue1.Value = 1;
            coursValue1.Enabled = true;
            button1.Visible = false; 
        }      
      }
}
