
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SC_Protagonist : MonoBehaviour
{
    public Button ButtonFood;
    public Button ButtonShower;
    public Button ButtonHand;
    public Button ButtonGun;

    public Texture2D HandCursorTexture;
    public Texture2D ShowerCursorTexture;
    public Texture2D FoodCursorTexture;
    public Texture2D GunCursorTexture;

    public AudioSource MusicSource;

    public Image WinTex;
    public Image LoseTex;

    private enum_ToolType ToolType = enum_ToolType.Shower;

    protected AudioSource ClickSoundSource;
    protected AudioSource WaterSplashSoundSource;
    protected AudioSource ShotSoundSource;

    protected AudioSource WinSoundSource;
    protected AudioSource LoseSounrSource;

    protected void PlaySound(AudioSource source)
    {
        if (source.isPlaying)
            return;
        source.Play();
    }

    // Use this for initialization
    void Start()
    {
        SetActionHand();

        ClickSoundSource = gameObject.AddComponent<AudioSource>();
        ClickSoundSource.clip = Resources.Load<AudioClip>("Click");

        WaterSplashSoundSource = gameObject.AddComponent<AudioSource>();
        WaterSplashSoundSource.clip = Resources.Load<AudioClip>("WaterSplash");
        WaterSplashSoundSource.volume = 0.4f;

        ShotSoundSource = gameObject.AddComponent<AudioSource>();
        ShotSoundSource.clip = Resources.Load<AudioClip>("Shot");
        ShotSoundSource.volume = 0.3f;

        WinSoundSource = gameObject.AddComponent<AudioSource>();
        WinSoundSource.clip = Resources.Load<AudioClip>("snd_Win");
        WinSoundSource.volume = 0.3f;

        LoseSounrSource = gameObject.AddComponent<AudioSource>();
        LoseSounrSource.clip = Resources.Load<AudioClip>("snd_Lose");
        LoseSounrSource.volume = 0.3f;
        MusicSource.mute = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            var hitResults = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hitResults.Length > 0)
            {
                Debug.Log("Result count " + hitResults.Length);
                OnMouseAction(hitResults);
            }
        }
        if(Input.GetKeyDown("q"))
        {
            SetActionHand();
        }
        else if (Input.GetKeyDown("w"))
        {
            SetActionFood();
        }
        else if (Input.GetKeyDown("e"))
        {
            SetActionShower();
        } 
        else if (Input.GetKeyDown("r"))
        {
            SetActionGun();
        }
    }

    public void SetShowerActive(bool isActive)
    {
        if (!isActive && ToolType == enum_ToolType.Shower)
            SetActionHand();
        ButtonShower.interactable = isActive;
    }

    public void SetFoodActive(bool isActive)
    {
        if (!isActive && ToolType == enum_ToolType.Food)
            SetActionHand();
        ButtonFood.interactable = isActive;
    }

    public void SetGunActive(bool isActive)
    {
        if (!isActive && ToolType == enum_ToolType.Gun)
            SetActionHand();
        ButtonGun.interactable = isActive;
        if (isActive)
        {
            //Проверяем, есть ли еще живые животные, если нет - конец игры            
            ChackVictoryConditions();
            SetActionGun();
        }
    }

    //Проверка, остался ли кто живой
    public void ChackVictoryConditions()
    {
        var animals = FindObjectsOfType<Animal>();
        bool success = true;
        foreach (var animal in animals)
        {
            if (!(animal is Girl) && animal.Health > 0.0f)
            {
                success = false;
                break;
            }
        }
        if (success)
        {
            //Game over
            var protagonist = FindObjectOfType<SC_Protagonist>();
            protagonist.FinishGame(true);
        }
    }

    public void FinishGame(bool isWin)
    {
        var dayNightController = FindObjectOfType<DayNightController>();
        dayNightController.IsLightOn = false;
        dayNightController.IsDay = false;
        dayNightController.UpdateRooms();
        Time.timeScale = 0.00001f;

        if (MusicSource != null)
            MusicSource.mute = true;

        if (isWin)
        {
            WinTex.enabled = true;
            PlaySound(WinSoundSource);
        }
        else
        {
            LoseTex.enabled = true;
            PlaySound(LoseSounrSource);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }

    public Sprite ToggleMusicOn;
    public Sprite ToggleMusicOff;
    public Toggle ToggleMusicB;
    public void ToggleMusic(bool newValue)
    {
        var ToggleMusicImg = ToggleMusicB.GetComponent<Image>();
        if (newValue)
            ToggleMusicImg.sprite = ToggleMusicOn;
        else
            ToggleMusicImg.sprite = ToggleMusicOff;

        if (MusicSource != null)
            MusicSource.mute = !newValue;
    }

    private void SetCursor(Texture2D texture)
    {
        var cursorHotspot = new Vector2(texture.width / 2, texture.height / 2);
        Cursor.SetCursor(texture, cursorHotspot, CursorMode.Auto);
    }

    public void SetActionHand()
    {
        if (ButtonHand.interactable)
        {
            ToolType = enum_ToolType.Hand;
            SetCursor(HandCursorTexture);
        }
    }

    public void SetActionShower()
    {
        if (ButtonShower.interactable)
        {
            ToolType = enum_ToolType.Shower;
            SetCursor(ShowerCursorTexture);
        }
    }

    public void SetActionFood()
    {
        if (ButtonFood.interactable)
        {
            ToolType = enum_ToolType.Food;
            SetCursor(FoodCursorTexture);
        }
    }

    public void SetActionGun()
    {
        if (ButtonGun.interactable)
        {
            ToolType = enum_ToolType.Gun;
            SetCursor(GunCursorTexture);
            ButtonFood.interactable = false;
            ButtonHand.interactable = false;
            ButtonShower.interactable = false;
        }
    }

    public void FixedUpdate()
    {

    }

    private const int PointEnergyPriority = 7;
    private const int Hippo70Priority = 6;
    private const int PorcupneGnawingPriority = 5;
    private const int DirtySpotPriority = 4;
    private const int Hippo30Priority = 3;
    private const int NeedlesPriority = 3;
    private const int MonkeyPriority = 2;
    private const int ItemPriority = 1;
    private const int HippoPriority = 1;

    private ICleanable GetItemToShower(RaycastHit2D[] hitResults)
    {
        ICleanable result = null;
        int currentPriority = 0;
        foreach (var hitResult in hitResults)
        {
            var cleanable = hitResult.collider == null ? null : hitResult.collider.gameObject.GetComponent<ICleanable>();
            if (cleanable == null || (cleanable is Animal && !(cleanable as Animal).enabled))
                continue;
            var prioroty = 0;
            if (cleanable is SC_Hippo)
            {
                var hippo = cleanable as SC_Hippo;
                var hippoPriority = HippoPriority;
                if (hippo.DirtyLevel > 0.7f)
                    hippoPriority = Hippo70Priority;
                else if (hippo.DirtyLevel > 0.3f)
                    hippoPriority = Hippo30Priority;
                prioroty = hippoPriority;
            }
            else if (cleanable is SC_PointEnergy)
            {
                var pointEnergy = cleanable as SC_PointEnergy;
                if (pointEnergy.IsLightBroken)
                    prioroty = PointEnergyPriority;
            }
            else if (cleanable is SC_Porcupine)
            {
                var porcupine = cleanable as SC_Porcupine;
                if (porcupine.GetIsGnawing())
                    prioroty = PorcupneGnawingPriority;
            }
            else if (cleanable is SC_Monkey)
            {
                var monkey = cleanable as SC_Monkey;
                if (monkey.GoToStealItem)
                    prioroty = MonkeyPriority;
            }
            else if (cleanable is DirtySpot)
            {
                prioroty = DirtySpotPriority;
            }

            if (prioroty > currentPriority)
            {
                result = cleanable;
                currentPriority = prioroty;
            }
        }
        return result;
    }

    private ITouchable GetItemToHand(RaycastHit2D[] hitResults)
    {
        ITouchable result = null;
        int currentPriority = 0;

        foreach (var hitResult in hitResults)
        {
            var touchable = hitResult.collider == null ? null : hitResult.collider.gameObject.GetComponent<ITouchable>();
            if (touchable == null || (touchable is Animal && !(touchable as Animal).enabled))
                continue;
            var prioroty = 0;
            if (touchable is SC_Monkey)
            {
                var monkey = touchable as SC_Monkey;
                if (monkey.ItemInHand != null)
                    prioroty = MonkeyPriority;
            }
            else if (touchable is SC_Needles)
            {
                prioroty = NeedlesPriority;
            }
            else if (touchable is SC_BaseMonkeyItem)
            {
                var item = touchable as SC_BaseMonkeyItem;
                if (item.CanTouch())
                    prioroty = ItemPriority;
            }

            if (prioroty > currentPriority)
            {
                result = touchable;
                currentPriority = prioroty;
            }
        }
        return result;
    }

    private Animal GetAnimalToFeed(RaycastHit2D[] hitResults)
    {
        Animal result = null;
        foreach (var hitResult in hitResults)
        {
            var animal = hitResult.collider == null ? null : hitResult.collider.gameObject.GetComponent<Animal>();

            if (animal != null && animal.enabled)
            {
                if (result == null || animal.Hunger > result.Hunger)
                {
                    Debug.Log("Result found!");
                    result = animal;
                }
            }
        }
        return result;
    }

    private Animal GetAnimalToKill(RaycastHit2D[] hitResults)
    {
        foreach (var hitResult in hitResults)
        {
            var animal = hitResult.collider == null ? null : hitResult.collider.gameObject.GetComponent<Animal>();
            if (animal != null && animal.enabled && animal.Health > 0)
            {
                return animal;
            }
        }
        return null;
    }

    public void OnMouseAction(RaycastHit2D[] hitResults)
    {
        switch (ToolType)
        {
            case enum_ToolType.Shower:

                var cleanable = GetItemToShower(hitResults);
                if (cleanable != null)
                {
                    PlaySound(WaterSplashSoundSource);
                    cleanable.Clean();
                }
                break;
            case enum_ToolType.Hand:
                var touchable = GetItemToHand(hitResults);
                if (touchable != null)
                {
                    PlaySound(ClickSoundSource);
                    touchable.Touch();
                }
                break;
            case enum_ToolType.Food:
                var animal = GetAnimalToFeed(hitResults);
                if (animal != null)
                    animal.FeedCreature();
                break;
            case enum_ToolType.Gun:
                var anim = GetAnimalToKill(hitResults);
                if (anim != null)
                {
                    PlaySound(ShotSoundSource);
                    anim.Kill();
                }
                break;
        }
    }
}
