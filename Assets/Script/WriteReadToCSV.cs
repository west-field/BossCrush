using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary> CSV�t�@�C���̏������݁A�ǂݍ��� </summary>
public class WriteReadToCSV : MonoBehaviour
{
    private List<string[]> readDataList = new List<string[]>();//�X�R�A�f�[�^���i�[���郊�X�g
    private List<string[]> writeDataList = new List<string[]>();//�X�R�A�f�[�^���i�[���郊�X�g
    private string[] currentData;//���ݕҏW���̃f�[�^
    private string filePath = "Assets/Score.csv";//csv�t�@�C���p�X
    private int lineNow;//���ݕҏW���Ă��郉�C��

    private void Start()
    {
        lineNow = 0;
    }

    /// <summary> �t�@�C���p�X��ݒ肷�� </summary>
    /// <param name="path">�p�X</param>
    public void FilePath(string path)
    {
        filePath = path;
    }

    //-----------------------Write-----------------------

    /// <summary> �f�[�^��ǉ����� </summary>
    public void AddData()
    {
        writeDataList.Add(currentData);
        currentData = new string[currentData.Length];//�V�����z��𐶐����āAcurrentData������������
    }

    /// <summary> CSV�t�@�C���Ƀf�[�^���������� </summary>
    public void WriteDataToCSV()
    {
        //�t�@�C�����Ȃ��Ƃ���
        if (!File.Exists(filePath))
        {
            //�t�@�C���쐬
            File.Create(filePath);
        }

        //CSV�t�@�C���ɏ�������
        StreamWriter writer = new StreamWriter(filePath, false);

        for (int i = lineNow; i < writeDataList.Count; i++)
        {
            string line = string.Join(",", writeDataList[i]);//�z����J���}�ŋ�؂���������ɕϊ�����
            writer.WriteLine(line);
            lineNow++;
        }
        writer.Flush();
        writer.Close();
    }

    /// <summary> �z����w��̒����ŏ���������(��s�ɂ����f�[�^��ݒ�ł���悤�ɂ��邩) </summary>
    /// <param name="length">�z��̒���</param>
    public void InitializeData(int length)
    {
        currentData = new string[length];
    }

    /// <summary> �w��̃C���f�b�N�X�Ƀf�[�^��ݒ肷�� </summary>
    /// <param name="index">�w��index</param>
    /// <param name="data">�f�[�^</param>
    public void SetDataAt(int index, int data)
    {
        currentData[index] = data.ToString();
    }

    //-----------------------Read-----------------------

    /// <summary> CSV�t�@�C���̃f�[�^��ǂݍ��� </summary>
    public void ReadDataToCSV()
    {
        //�t�@�C�������鎞
        if(File.Exists(filePath))
        {
            //CSV�t�@�C����ǂݍ���
            StreamReader reader = new StreamReader(filePath);

            while (reader.Peek() != -1)
            {
                var line = reader.ReadLine();//��s���ǂݍ���
                readDataList.Add(line.Split(','));//���X�g�ɒǉ�����
            }

            reader.Close();
        }
    }

    /// <summary> �f�[�^���擾 </summary>
    /// <returns></returns>
    public List<string[]> Data()
    {
        return readDataList;
    }

}
