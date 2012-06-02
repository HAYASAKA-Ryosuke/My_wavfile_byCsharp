using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wavfiletest
{
    class WaveGenerator
    {
        byte[] wav;
        int length;
        int ch_flg = 2;
        int samplingrate=44100;
   
       public WaveGenerator(byte[] data)
        {
            this.length = data.Length + 20 + 4 + 8 + 6 + 8;//生データとヘッダー分を含めた長さ(ファイルサイズ)
            this.wav = data;//wavに入力データを代入
        }

       public WaveGenerator(byte[] data,int ch)
        {
            this.length = data.Length + 20 + 4 + 8 + 6 + 8;//生データとヘッダー分を含めた長さ(ファイルサイズ)
            this.wav = data;//wavに入力データを代入
            /*
             *ステレオまたはモノラルを設定
             */
            if (ch == 1)
                this.ch_flg = ch;
            else
                this.ch_flg = 2;
        }
       public WaveGenerator(byte[] data, int ch,int samplingrate)
        {
            this.length = data.Length + 20 + 4 + 8 + 6 + 8;//生データとヘッダー分を含めた長さ(ファイルサイズ)
            this.wav = data;//wavに入力データを代入
            this.samplingrate = samplingrate;
            /*
             *ステレオまたはモノラルを設定
             */
            if (ch == 1)
                this.ch_flg = ch;
            else
                this.ch_flg = 2;
        }
      
        public byte[] WavefileReturn()
        {
            if (ch_flg == 1)
               return wavefilegener(this.wav, this.wav.Length, 1,this.samplingrate);
            else
               return wavefilegener(this.wav, this.wav.Length, 2,this.samplingrate);
        }
        private byte[] wavefilegener(byte[] data, int length, int ch,int rate)
        {
            byte[] wave = new byte[length + 20 + 4 + 8 + 6 + 8];
            int filelength = length + 20 + 4 + 8 + 6 + 8;
            int samplingrate = rate;
            int samplingbit = 16;
            wave[0] = 0x52; wave[1] = 0x49; wave[2] = 0x46; wave[3] = 0x46;//RIFF(決まり文句)
            wave[4] = Dec_to_Hex(filelength)[0]; wave[5] = Dec_to_Hex(filelength)[1]; wave[6] = Dec_to_Hex(filelength)[2]; wave[7] = Dec_to_Hex(filelength)[3];//ファイルサイズ(88246byte)
            //   wav[4] = 0xB6; wav[5] = 0x58; wav[6] = 0x01; wav[7] = 0x00;//ファイルサイズ(88246byte)
            wave[8] = 0x57; wave[9] = 0x41; wave[10] = 0x56; wave[11] = 0x45;//WAVE(決まり文句)
            wave[12] = 0x66; wave[13] = 0x6D; wave[14] = 0x74; wave[15] = 0x20;//fmt (決まり文句)注:最後はスペース
            wave[16] = Dec_to_Hex(16)[0]; wave[17] = Dec_to_Hex(16)[1]; wave[18] = Dec_to_Hex(16)[2]; wave[19] = Dec_to_Hex(16)[3];
            //  wav[16] = 0x10; wav[17] = 0x00; wav[18] = 0x00; wav[19] = 0x00;//dataの数までのPCMWAVEFORMAT情報領域のサイズ(16byte)
            wave[20] = 0x01; wave[21] = 0x00;//waveファイルならこの値(大人の事情で固定ということにする)
            if (ch == 1) { wave[22] = 0x01; wave[23] = 0x00; }//1:モノラル,2:ステレオ今回はモノラルにしておく}
            else { wave[22] = 0x02; wave[23] = 0x00; }//1:モノラル,2:ステレオ今回はステレオにしておく
            wave[24] = Dec_to_Hex(samplingrate)[0]; wave[25] = Dec_to_Hex(samplingrate)[1]; wave[26] = Dec_to_Hex(samplingrate)[2]; wave[27] = Dec_to_Hex(samplingrate)[3];
            //wav[24] = 0x44; wav[25] = 0xAC; wav[26] = 0x00; wav[27] = 0x00;//サンプリング周波数44100Hzを表す
            wave[28] = Dec_to_Hex(samplingrate * ch * samplingbit / 8)[0]; wave[29] = Dec_to_Hex(samplingrate * ch * samplingbit / 8)[1]; wave[30] = Dec_to_Hex(samplingrate * ch * samplingbit / 8)[2]; wave[31] = Dec_to_Hex(samplingrate * ch * samplingbit / 8)[3];
            //wav[28] = 0x10; wav[29] = 0xB1; wav[30] = 0x02; wav[31] = 0x00;//平均データ転送レート(式:サンプリングレート(44.1kHz) * チャンネル(2ch) * サンプリングビット(16bit) / 8=176400Hz)
            if (ch == 1) { wave[32] = 0x02; wave[33] = 0x00; }
            else { wave[32] = 0x04; wave[33] = 0x00; }//データ転送のための最小単位(4byte)今回は16ビット(2byte)でステレオで書き込むので4byte必要
            wave[34] = 0x10; wave[35] = 0x00;//サンプリングビット(16bit=2byte)
            wave[36] = 0x64; wave[37] = 0x61; wave[38] = 0x74; wave[39] = 0x61;//DATA(決まり文句)
            wave[40] = Dec_to_Hex(length)[0]; wave[41] = Dec_to_Hex(length)[1]; wave[42] = Dec_to_Hex(length)[2]; wave[43] = Dec_to_Hex(length)[3];
            //wav[40] = 0x88; wav[41] = 0x58; wav[42] = 0x01; wav[43] = 0x00;//書き込む音声データのサイズ(88200byte)
            for (int i = 44; i < data.Length; i++)
            {
                wave[i] = data[i - 44];//音声データをヘッダに結合
            }
            return wave;
        }

        private byte[] Dec_to_Hex(int value)
        {
            byte[] data = new byte[4];
            string bytestr = "";
            for (int i = 0; i < 8 - value.ToString("X").Length; i++)
            {
                bytestr += "0";
                //     bytestr.PadLeft(1, '0');
            }
            bytestr += value.ToString("X");
            data[0] = Convert.ToByte(bytestr.Substring(6, 2), 16);
            //data[0] = Convert.ToByte("B6");
            //  bytestr2.Substring(4, 5);
            data[1] = Convert.ToByte(bytestr.Substring(4, 2), 16);
            data[2] = Convert.ToByte(bytestr.Substring(2, 2), 16);
            data[3] = Convert.ToByte(bytestr.Substring(0, 2), 16);

            return data;
        }
    }
}
