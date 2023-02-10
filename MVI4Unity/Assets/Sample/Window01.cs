using UnityEngine;
using UnityEngine.UI;

namespace MVI4Unity
{
    public class Window01 : AWindow
    {
        [AWindowCom ("container1")]
        public Transform container1;

        [AWindowCom ("container2")]
        public Transform container2;

        [AWindowCom ("btn")]
        public Button btn;
    }
}