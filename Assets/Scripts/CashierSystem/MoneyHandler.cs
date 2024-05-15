using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoneyHandler : MonoBehaviour
{
    public Game_Over_2_GameInteraction m_GOM;
    [SerializeField] private int StartRange;
    [SerializeField] private int EndRange;
    [SerializeField] private Sprite[] CashSprites;
    [SerializeField] private Button CheckReturnedMoney;
    [SerializeField] private Button ResetReturnedMoneySlot;
    [SerializeField] private Text RestValueText;
    private int Price;
    private int CustomerGivenMoney;
    private int MoneyToReturn;
    private string PriceString;
    //public GameObject PriceContainer;
    public GameObject ReturnedMoneyContainer;
    //public GameObject CustomerGivenedMoneyContainer;
    public Transform AddedMoneyValueContainer;
    public int NumberOfGivenPaper;
    [SerializeField] Text ReturnedMoneyTextValue;

    // Start is called before the first frame update
    void Start()
    {
        Price= GenerateRandomNumber();
      
        CustomerGivenMoney=(GenerateNumber(Price));
        MoneyToReturn = CustomerGivenMoney - Price;
        RestValueText.text = MoneyToReturn.ToString();
        Sprite[] MoneyToRepresent = SelectSprites(CustomerGivenMoney);
        //InstintiateMoneyDropSlot(NumberOfNeededCashItems(MoneyToReturn+AddedPassValue));
        CheckReturnedMoney.onClick.AddListener(() => CheckIfReturnedMoeyIsCorrect());
        ResetReturnedMoneySlot.onClick.AddListener(() => DestroyAllRetunedMoneyContainerChildren());
        
    }

    // Function to dynamically space the children of a GameObject within a grid layout
    public void SpaceChildren(GameObject parentGO)
    {
        // Get the RectTransform of the parent GameObject
        RectTransform parentRect = parentGO.GetComponent<RectTransform>();

        // Calculate the number of columns and rows
        int columns = Mathf.CeilToInt(Mathf.Sqrt(parentGO.transform.childCount));
        int rows = Mathf.CeilToInt((float)parentGO.transform.childCount / columns);

        // Calculate the spacing between children
        float horizontalSpacing = parentRect.sizeDelta.x / columns;
        float verticalSpacing = parentRect.sizeDelta.y / rows;

        // Position the children within the grid layout
        int childIndex = 0;
        for (int rowIndex = 0; rowIndex < rows; rowIndex++)
        {
            for (int colIndex = 0; colIndex < columns; colIndex++)
            {
                if (childIndex >= parentGO.transform.childCount)
                    return;

                Transform child = parentGO.transform.GetChild(childIndex);
                RectTransform childRect = child.GetComponent<RectTransform>();

                // Calculate position based on grid layout
                float xPos = colIndex * horizontalSpacing + horizontalSpacing / 2f - childRect.sizeDelta.x / 2f;
                float yPos = rowIndex * verticalSpacing + verticalSpacing / 2f - childRect.sizeDelta.y / 2f;

                // Apply position
                childRect.anchoredPosition = new Vector2(xPos, yPos);

                childIndex++;
            }
        }
    }
    public int GenerateNumber(int price)
    {
        int generatedNumber;

        // Apply rules based on the value of price
        if (price < 10000)
        {
            generatedNumber = Random.Range(0, 3) switch
            {
                0 => 20000,
                1 => 10000,
                _ => 50000
            };
        }
        else if (price < 15000)
        {
            generatedNumber = Random.Range(0, 3) switch
            {
            
                0 => 20000,
                _ => 50000
            };
        }
        else if (price < 20000)
        {
            generatedNumber = Random.Range(0, 2) switch
            {
                0 => 20000,
                _ => 50000
            };
        }
        else if (price < 25000)
        {
            generatedNumber = Random.Range(0, 3) switch
            {
                0 => 30000,
                1 => 40000,
                _ => 50000,
              
            };
        }
        else if (price < 30000)
        {
            generatedNumber = Random.Range(0, 3) switch
            {
                0 => 30000,
                1 => 40000,
                _ => 50000
            };
        }
        else if (price < 35000)
        {
            generatedNumber = Random.Range(0, 2) switch
            {
           
                0 => 40000,
                _ => 50000
            };
        }
        else if (price < 40000)
        {
            generatedNumber = Random.Range(0, 2) switch
            {
                0 => 40000,
                _ => 50000
            };
        }
        else if (price < 45000)
        {
            generatedNumber = Random.Range(0, 1) switch
            {
                _ => 50000,
        
            };
        }
        else if (price < 50000)
        {
            generatedNumber = 50000;
        }
        else
        {
            // If price is greater than 50000, add (price - 50000) to 50000
            /*            generatedNumber = 50000 + GenerateNumber(price - 50000);

            */
            generatedNumber = 100000 ;
        }

        return generatedNumber;
    }
    int GenerateRandomNumber()
    {
        // Creating a random number generator
        System.Random random = new System.Random();

        // Generating a random number between 5000 and 10000
        int randomNumber = random.Next(StartRange, EndRange);
        // Multiplying by 10 to ensure the number ends with 0
        //int numberEndingWithZero = randomNumber * 10;

        return randomNumber;
    }
    // Function to select sprites representing the minimum combination of money papers or coins
    public Sprite[] SelectSprites(int value)
    {
        Debug.Log(value);
        // Array to store the selected sprites
        Sprite[] selectedSprites = new Sprite[2];
        // Loop through the CashSprites array in normal order
        for (int i = 0; i < CashSprites.Length; i++)
        {
            // Calculate the number of times the current money paper or coin can be used
            int count = value / GetMoneyValue(i);
            if (count>0)
            {
                for (int j = 0; j < count; j++)
                {
                    selectedSprites[j] = CashSprites[i];
                }

                value -= count * GetMoneyValue(i);

            }
            if (value==0)
            {
                return selectedSprites;
            }
        }
        return selectedSprites;
    }
    public int PlayerReturnedMoneyValue()
    {
        int MoneyValue=0;
        foreach (Transform child in ReturnedMoneyContainer.transform)
        {
            // Get the Image component
            Image imageComponent = child.GetComponent<Image>();
            Sprite sprite = imageComponent.sprite;

                // Do something with the sprite
                if (sprite != null)
                {
                    MoneyValue += GetMoneyValue(FindSpriteIndex(sprite));
                    Debug.Log("Sprite found: " + sprite.name);
                }
       
        }
        return MoneyValue;
    }
    public void DestroyAllRetunedMoneyContainerChildren()
    {
            // Loop through each child of the parent GameObject
            foreach (Transform child in ReturnedMoneyContainer.transform)
            {
                // Destroy the child GameObject
                Destroy(child.gameObject);
            }
            foreach (Transform child in AddedMoneyValueContainer)
            {
                // Destroy the child GameObject
                Destroy(child.gameObject);
            }
            ReturnedMoneyTextValue.text = "0";

    }
    public void CheckIfReturnedMoeyIsCorrect()
    {
        if (PlayerReturnedMoneyValue()== MoneyToReturn)
        {
            Debug.Log("CorrectReturnedMoney");
            //m_GOM.Game_Over(true, 3);
            //GameManager.instance.RestartScene();
        }
        else
        {
            Debug.Log("IncorrectReturnedMoney");

        }
    }
    public int FindSpriteIndex(Sprite spriteToFind)
    {
        // Find the index of the sprite in the array
        int index = System.Array.IndexOf(CashSprites, spriteToFind);

        // Return the index
        return index;
    }
    public int NumberOfNeededCashItems(int moneyValue)
    {
        int numberOfNeededCashItems=0;
        // Loop through the CashSprites array in normal order
        for (int i = 0; i < CashSprites.Length; i++)
        {
            // Calculate the number of times the current money paper or coin can be used
            int count = moneyValue / GetMoneyValue(i);
            numberOfNeededCashItems += count;
            moneyValue -= count * GetMoneyValue(i);
        }
        return numberOfNeededCashItems;
    }
    // Function to get the monetary value represented by the sprite at the specified index
    public int GetMoneyValue(int index)
    {
        switch (index)
        {
            case 0: return 50000;
            case 1: return 20000;
            case 2: return 10000;
            case 3: return 5000;
            case 4: return 2000;
            case 5: return 1000;
            case 6: return 500;
            case 7: return 200;
            case 8: return 100;
            case 9: return 50;
            case 10: return 20;
            case 11: return 10;
            case 12: return 5;
            case 13: return 2;
            case 14: return 1;
            default: return 0;
        }
    }
    
}
