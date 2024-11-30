using System;
using System.IO;
using UnityEngine;
public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string _dataDirPath, string _dataFilePath)//���캯���õ���Ҫ�����λ�ú��ļ�����
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFilePath;
    }

    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);//�ϳ�·������ ��λ�ú��ļ��ϲ���ʵ�ʵĿ��Զ�ȡ��·��

        try//��try��ֹ�䱨��
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));//ͨ��·����������Ҫ���ļ������ھͲ�������
            string dataToStore = JsonUtility.ToJson(_data, true);//����������gameDataת�����ı���ʽ����ʹ��ɶ�

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))//����using ��һ�������ļ�ʹ���Ϊ�ɱ�дģʽ
            {
                using (StreamWriter writer = new StreamWriter(stream))//�ڶ����õ��ļ�������б༭
                {
                    writer.Write(dataToStore);//д�뺯��
                }
            }

        }

        catch (Exception e)
        {
            Debug.LogError("Error on trying to save data to file " + fullPath + "\n" + e);
        }
    }
    public GameData Load()//ͬ��
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadData = JsonUtility.FromJson<GameData>(dataToLoad);//ת��Ϊ��Ϸ��Ҫ������
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return loadData;
    }

    public void Delete()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if (File.Exists(fullPath))
        {
             File.Delete(fullPath);
        }
    }
}