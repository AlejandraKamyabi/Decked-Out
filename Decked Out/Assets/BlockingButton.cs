using UnityEngine;
using UnityEngine.EventSystems;

public class BlockingButton : MonoBehaviour, IPointerEnterHandler
{
    private GameLoader _loader;
    private CardRandoEngine _randoEngine;
    private MouseInputHandling _mouseInput;
    private TowerSelection _towerSelection;

    float _lastSpellSlot;
    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }
    private void Initialize()
    {
        _randoEngine = FindObjectOfType<CardRandoEngine>();
        _mouseInput = FindObjectOfType<MouseInputHandling>();
        _towerSelection = FindObjectOfType<TowerSelection>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Entry");
        if (_towerSelection.isSelectingTower == true)
        {
            _towerSelection.SetSelectingTower(false);
            _mouseInput.ClearRig();
            PutCardBack();
        }
        else if (_towerSelection.isSelectingSpell == true)
        {
            _towerSelection.SetSelectingSpell(false);
            _mouseInput.ClearRig();
            _lastSpellSlot = _randoEngine.SpellSlotCheck();
            PutCardBack();
        }
        else
        {
           Debug.LogError("Blocking Button up but Tower Selection is not selecting a tower");
        }
    }
    private void PutCardBack()
    {
        if (_randoEngine._lastCardSlot.activeInHierarchy == false)
        {
            _randoEngine._lastCardSlot.SetActive(true);
            _randoEngine._lastCardSlot = null;
        }
    }

}
