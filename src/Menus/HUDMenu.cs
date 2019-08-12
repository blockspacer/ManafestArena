using Godot;
using System;
using System.Collections.Generic;

public class HUDMenu : Container, IMenu{

  public float delay = 0.0f;
  public Godot.Label healthBox;
  public Godot.Label itemBox;
  public Godot.Label objectiveBox;
  public Godot.Label interactionBox;
  public Godot.Label drawTimeBox;
  public Godot.Label[] cardBoxes;
  
  public override void _Process(float delta){
    delay += delta;

    if(delay > 0.033f){
      delay -= 0.033f;
      Update();
    }
  }

  public void Init(){
    Input.SetMouseMode(Input.MouseMode.Captured);
    InitControls();

    ScaleControls();
    GetTree().GetRoot().Connect("size_changed", this, "ScaleControls");

    //UpdateHandOfCards(new List<string>{"strike", "strike", "strike", "defend", "defend"});
  }

  public void Update(){
    Actor player = Session.GetPlayer();
    
    if(player == null){
      GD.Print("Player 1 doesn't exist.");
      return;
    }
    
    healthBox.Text = StatusText(player);

    string itemText = player.hotbar.GetInfo();

    itemBox.Text = itemText;
    
    string objectiveText = Session.GetObjectiveText();
    objectiveBox.Text = objectiveText;
  }

  private string StatusText(Actor player){
    if(player.stats == null){
      return "";
    }
    IStats stats = player.stats;

    string ret = "";
    ret += "Block: " + stats.GetStat("block");
    ret += "\nHealth: " + stats.GetStat("health") + "/" + stats.GetStat("healthmax");
    ret += "\nEnergy: " + stats.GetStat("stamina") + "/" + stats.GetStat("staminamax");
    return ret;

  }

  public void ToggleHandOfCards(bool active){
    // for(int i = 0; i < 5; i++){
    //   cardBoxes[i].Visible = active;
    // }

    Rect2 screen = this.GetViewportRect();
      float width = screen.Size.x;
      float height = screen.Size.y;
      float wu = width/10; // relative height and width units
      float hu = height/10;

    if(active){
      Menu.ScaleControl(cardBoxes[0], wu, 2* hu, 3 * wu, height - hu);
      Menu.ScaleControl(cardBoxes[1], wu, 2* hu, 4 * wu, height - hu);
      Menu.ScaleControl(cardBoxes[2], wu, 2* hu, 5 * wu, height - hu);
      Menu.ScaleControl(cardBoxes[3], wu, 2* hu, 6 * wu, height - hu);
      Menu.ScaleControl(cardBoxes[4], wu, 2* hu, 7 * wu, height - hu);
    }
    else{
      Menu.ScaleControl(cardBoxes[0], wu, 2* hu, 3 * wu, height - 0.3f*hu);
      Menu.ScaleControl(cardBoxes[1], wu, 2* hu, 4 * wu, height - 0.3f*hu);
      Menu.ScaleControl(cardBoxes[2], wu, 2* hu, 5 * wu, height - 0.3f*hu);
      Menu.ScaleControl(cardBoxes[3], wu, 2* hu, 6 * wu, height - 0.3f*hu);
      Menu.ScaleControl(cardBoxes[4], wu, 2* hu, 7 * wu, height - 0.3f*hu);
    }
  }

  public void UpdateHandOfCards(List<string> cards){
    ClearHandOfCards();

    for(int i = 0; i < 5; i++){
      if(cards.Count > i){
        cardBoxes[i].Text = cards[i];
      }
    }

  }


  public void UpdateDrawTime(int count){
    drawTimeBox.Text = "Next Card in (" + count + ")"; 
  }

  public void ClearHandOfCards(){
      for(int i = 0; i < 5; i++){
        cardBoxes[i].Text = "";
      }
  }

  void InitControls(){
    cardBoxes = new Godot.Label[5];
    for(int i = 0; i < 5; i++){
      cardBoxes[i] = Menu.Label("");
      AddChild(cardBoxes[i]);
    }

    drawTimeBox = Menu.Label("DrawPile(0)");
    AddChild(drawTimeBox); 

    healthBox = Menu.Label("health");
    AddChild(healthBox);

    itemBox = Menu.Label("item");
    AddChild(itemBox);
    
    objectiveBox = Menu.Label("Objective Info");
    AddChild(objectiveBox);

    interactionBox = Menu.Label("");
    AddChild(interactionBox);
  }

  public void ScaleControls(){
    Rect2 screen = this.GetViewportRect();
    float width = screen.Size.x;
    float height = screen.Size.y;
    float wu = width/10; // relative height and width units
    float hu = height/10;

    Menu.ScaleControl(healthBox, 2 * wu, hu,  wu, height - hu);
    Menu.ScaleControl(objectiveBox, 4 * wu, hu, 3 * wu, 0);
    Menu.ScaleControl(interactionBox, 4 * wu, hu, 3 * wu, 7 * hu);
    Menu.ScaleControl(drawTimeBox, 2 * wu, hu, 0, height - hu);
    Menu.ScaleControl(itemBox, 2 * wu, hu, 8 * wu, height - hu);
    Menu.ScaleControl(cardBoxes[0], wu, 2* hu, 3 * wu, height - hu);
    Menu.ScaleControl(cardBoxes[1], wu, 2* hu, 4 * wu, height - hu);
    Menu.ScaleControl(cardBoxes[2], wu, 2* hu, 5 * wu, height - hu);
    Menu.ScaleControl(cardBoxes[3], wu, 2* hu, 6 * wu, height - hu);
    Menu.ScaleControl(cardBoxes[4], wu, 2* hu, 7 * wu, height - hu);
  }
}
