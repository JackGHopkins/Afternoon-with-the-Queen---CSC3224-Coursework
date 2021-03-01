using Godot;
using System;

public class DropdownButton : OptionButton
{
    [Export] String[] Options;

    public override void _Ready()
    {
        AddItems(Options);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void AddItems(String[] Options){
        foreach( string i in Options)
            this.AddItem(i);
    }
}
