using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ANR
{
    public class SemanticSentenceLoader : MonoBehaviour
    {
        #region Properties

        [SerializeField] private TextAsset semanticSentenceJSON;

        private SemanticSentence[] semanticSentences;

        #endregion

        void Awake()
        {
            //Debug.Log(semanticSentenceJSON.text);

            // load json (only for small files)
            semanticSentences = JsonHelper.FromJson<SemanticSentence>(semanticSentenceJSON.text);

            // alternative: using StreamingAssets
            //string path = Application.streamingAssetsPath + "/SemanticSentences_de_01.json";
            //string jsonString = File.ReadAllText(path);
            //semanticSentences = JsonHelper.FromJson<SemanticSentence>(jsonString);
        }

        // Start is called before the first frame update
        void Start()
        {
            if (!semanticSentenceJSON)
            {
                Debug.LogError("semanticSentenceJSON must not be null");
                return;
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        public SemanticSentence[] GetSemanticSentences()
        {
            //Debug.Log(semanticSentenceJSON.text);
            //SemanticSentence[] semanticSentences = JsonHelper.FromJson<SemanticSentence>(semanticSentenceJSON.text);

            Debug.Log("allLines: " + semanticSentences.Length);
            Debug.Log("line 1 sentence = " + semanticSentences[0].sentence + ", correct = " +
                      semanticSentences[0].correct);

            return semanticSentences;
        }
    }
}