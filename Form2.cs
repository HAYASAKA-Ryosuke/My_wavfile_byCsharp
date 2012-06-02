using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace wavfiletest
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            datacreator();
            WaveGenerator wg = new WaveGenerator(data, 2,48000);
            //ファイルを作成して書き込む
            //ファイルが存在しているときは、上書きする
            System.IO.FileStream fs = new System.IO.FileStream(
                @"C:\test.wav",
                System.IO.FileMode.Create,
                System.IO.FileAccess.Write);
            //バイト型配列の内容をすべて書き込む
            fs.Write(wg.WavefileReturn(), 0, wg.WavefileReturn().Length);
            //閉じる
            fs.Close();
            
            
        }
        byte[] data=new byte[48000*2];
        int sampling = 48000;
        int F = 400;//400Hzの音
        int len = 48000 / 400;//波長
        //波形ジェネレータ今回は適当にsin波を書き込んでみる
        private void datacreator()
        {
            for (int i = 0; i < sampling * 2; i++)
            {
                if (i % len < len / 2) data[i] = 128 + 64;
                else data[i] = 128 - 64;
            }
            int j = 0;
        }
    }
}
