Wave�t�@�C���𐶂̉����f�[�^���琶�����Ă����N���X�t�@�C���ł��D

�ȒP�Ȏg������
byte[] data;//���̐M���f�[�^
byte[] wavefile;//wave�����ꂽ�f�[�^���i�[����z��

//�R���X�g���N�^�Ăяo�����ɐ��f�[�^��������Ɏw�肪������΃X�e���I�C�T���v�����O���[�g:44.1kHz
WaveGenerator wg=new WaveGenerator(data);
//Wave�����ꂽ�f�[�^���Ăт���
wavefile = wg.WavefileReturn();

