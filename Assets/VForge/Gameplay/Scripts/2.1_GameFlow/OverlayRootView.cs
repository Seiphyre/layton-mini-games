using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VForge.Gameplay
{
    public class OverlayRootView : MonoBehaviour
    {
        [SerializeField] private ResultPopupView resultPopupView;

        public ResultPopupView ResultPopupView => resultPopupView;


        private void Awake()
        {
            HideAll();
        }



        public void HideAll()
        {
            resultPopupView.Hide();
            gameObject.SetActive(false);
        }

        public void ShowResult(VictoryValidationResult result)
        {
            HideAll();
            resultPopupView.Show(result);
            gameObject.SetActive(true);
        }
    }
}
