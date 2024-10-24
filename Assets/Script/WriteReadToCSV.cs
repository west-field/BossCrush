using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary> CSVファイルの書き込み、読み込み </summary>
public class WriteReadToCSV : MonoBehaviour
{
    private List<string[]> readDataList = new List<string[]>();//スコアデータを格納するリスト
    private List<string[]> writeDataList = new List<string[]>();//スコアデータを格納するリスト
    private string[] currentData;//現在編集中のデータ
    private string filePath = "Assets/Score.csv";//csvファイルパス
    private int lineNow;//現在編集しているライン

    private void Start()
    {
        lineNow = 0;
    }

    /// <summary> ファイルパスを設定する </summary>
    /// <param name="path">パス</param>
    public void FilePath(string path)
    {
        filePath = path;
    }

    //-----------------------Write-----------------------

    /// <summary> データを追加する </summary>
    public void AddData()
    {
        writeDataList.Add(currentData);
        currentData = new string[currentData.Length];//新しい配列を生成して、currentDataを初期化する
    }

    /// <summary> CSVファイルにデータを書き込む </summary>
    public void WriteDataToCSV()
    {
        //ファイルがないときは
        if (!File.Exists(filePath))
        {
            //ファイル作成
            File.Create(filePath);
        }

        //CSVファイルに書き込む
        StreamWriter writer = new StreamWriter(filePath, false);

        for (int i = lineNow; i < writeDataList.Count; i++)
        {
            string line = string.Join(",", writeDataList[i]);//配列をカンマで区切った文字列に変換する
            writer.WriteLine(line);
            lineNow++;
        }
        writer.Flush();
        writer.Close();
    }

    /// <summary> 配列を指定の長さで初期化する(一行にいくつデータを設定できるようにするか) </summary>
    /// <param name="length">配列の長さ</param>
    public void InitializeData(int length)
    {
        currentData = new string[length];
    }

    /// <summary> 指定のインデックスにデータを設定する </summary>
    /// <param name="index">指定index</param>
    /// <param name="data">データ</param>
    public void SetDataAt(int index, int data)
    {
        currentData[index] = data.ToString();
    }

    //-----------------------Read-----------------------

    /// <summary> CSVファイルのデータを読み込む </summary>
    public void ReadDataToCSV()
    {
        //ファイルがある時
        if(File.Exists(filePath))
        {
            //CSVファイルを読み込む
            StreamReader reader = new StreamReader(filePath);

            while (reader.Peek() != -1)
            {
                var line = reader.ReadLine();//一行ずつ読み込む
                readDataList.Add(line.Split(','));//リストに追加する
            }

            reader.Close();
        }
    }

    /// <summary> データを取得 </summary>
    /// <returns></returns>
    public List<string[]> Data()
    {
        return readDataList;
    }

}
