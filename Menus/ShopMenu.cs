using Godot;
using System;
using System.Collections.Generic;

public class ShopMenu : Container, IMenu {
  public Button finishedButton;
  public List<Button> itemButtons;
  public System.Collections.Generic.Dictionary<string, ItemData> itemsDict;


  public void Init(float minX, float minY, float maxX, float maxY){
    InitControls();
    ScaleControls();
    GetTree().GetRoot().Connect("size_changed", this, "ScaleControls");
  }
  
  public void Resize(float minX, float minY, float maxX, float maxY){
    ScaleControls();
  }

  public bool IsSubMenu(){
    return false;
  }

  public void Clear(){
    this.QueueFree();
  }

  void InitControls(){
    finishedButton = Menu.Button("Finish shopping", () => { 
      Session.session.career.CompleteEncounter();
    });
    AddChild(finishedButton);


    itemButtons = new List<Button>();
    List<ItemData> items = Career.ShopItems();
    itemsDict = new System.Collections.Generic.Dictionary<string, ItemData>();

    foreach(ItemData item in items){
      string shopName = item.extra["shop_name"];
      Button itemButton = Menu.Button(item.name, () => {
        PurchaseItem(shopName);
      });

      AddChild(itemButton);
      itemButtons.Add(itemButton);
      
      if(!itemsDict.ContainsKey(shopName)){
        itemsDict.Add(shopName, item);
      }
    }
  }

  void PurchaseItem(string name){
    if(!itemsDict.ContainsKey(name)){
      GD.Print("itemsDict Doesn't conttain " + name);
      return;
    }

    ItemData item = itemsDict[name];
    GD.Print("Purchasing  " + item.ToString());
  }


  void ScaleControls(){
    Rect2 screen = this.GetViewportRect();
    float width = screen.Size.x;
    float height = screen.Size.y;
    float wu = width/10; // relative height and width units
    float hu = height/10;

    Menu.ScaleControl(finishedButton, 2 * wu, hu, width - (2 * wu), height - hu);

    for(int i = 0; i < 4; i++){
      Menu.ScaleControl(itemButtons[i], 2 * wu, 2 * hu, i * 2 * wu, 2 * hu);
    }
    for(int i = 4; i < 8; i++){
      int off = i - 4;
      Menu.ScaleControl(itemButtons[i], 2 * wu, 2 * hu, off * 2 * wu, 4 * hu); 
    }
    
  }

}