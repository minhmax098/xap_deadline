using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using System; 
using UnityEngine.Networking;

namespace LessonDetail_Edit
{
    public class LoadScene : MonoBehaviour
    {
        public LessonDetail[] myData; 
        public LessonDetail currentLesson; 
        public GameObject bodyObject; 
        public GameObject lessonTitle; 
        public int calculatedSize = 15;

        void Start()
        {
            Screen.orientation = ScreenOrientation.Portrait; 
            StatusBarManager.statusBarState = StatusBarManager.States.TranslucentOverContent;
            StatusBarManager.navigationBarState = StatusBarManager.States.Hidden;
            
            Debug.Log("Lesson ID:");
            Debug.Log(LessonManager.lessonId);

            myData = LoadData.Instance.GetLessonsByID(LessonManager.lessonId.ToString()).data; 
            // currentLesson = Array.Find(myData, lesson => lesson.lessonId == LessonManager.lessonId); 
            currentLesson = myData[0];
            StartCoroutine(LoadCurrentLesson(currentLesson));
        }

        IEnumerator LoadCurrentLesson(LessonDetail currentLesson)
        {
            string imageUri = String.Format(APIUrlConfig.LoadLesson, currentLesson.lessonThumbnail);  
            lessonTitle.gameObject.GetComponent<Text>().text = Helper.FormatString(currentLesson.lessonTitle.ToLower(), calculatedSize);
            bodyObject.transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = Helper.FormatString(currentLesson.authorName, calculatedSize);
            bodyObject.transform.GetChild(2).GetChild(0).GetChild(1).GetChild(1).GetComponent<Text>().text = DateTime.Parse(currentLesson.createdDate).ToString("dd/MM/yyyy HH:mm:ss");
            bodyObject.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>().text = "#" + currentLesson.lessonId.ToString();
            bodyObject.transform.GetChild(2).GetChild(1).GetChild(1).GetComponent<Text>().text = currentLesson.viewed.ToString() + " Views"; 
            
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUri);
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {

            }
            if (request.isDone)
            {
                Texture2D tex = ((DownloadHandlerTexture) request.downloadHandler).texture;
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
                bodyObject.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = sprite;
                bodyObject.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
