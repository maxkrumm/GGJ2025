using Ricimi;
using UnityEngine;

public class FadePopupOpener : PopupOpener
{
    public override Popup OpenPopup()
    {
        var popup = Instantiate(popupPrefab) as GameObject;
        popup.SetActive(true);
        popup.transform.SetParent(m_canvas.transform, false);
        var comp = popup.GetComponent<Popup>();
        comp.Open();
        return comp;
    }

}
