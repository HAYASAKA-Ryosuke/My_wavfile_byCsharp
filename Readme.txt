Waveファイルを生の音声データから生成してくれるクラスファイルです．

簡単な使い方例
byte[] data;//生の信号データ
byte[] wavefile;//wave化されたデータを格納する配列

//コンストラクタ呼び出し時に生データを入れる特に指定が無ければステレオ，サンプリングレート:44.1kHz
WaveGenerator wg=new WaveGenerator(data);
//Wave化されたデータを呼びだす
wavefile = wg.WavefileReturn();

