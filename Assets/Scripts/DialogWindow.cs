﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;

// Class for creation of a dialog window with buttons and handling button press
public class DialogWindow {
    // The even that raises this dialog
    public QuestData.Event eventData;

    public DialogWindow(QuestData.Event e)
    {
        eventData = e;
        CreateWindow();
    }

    public void CreateWindow()
    {
        new DialogBox(new Vector2(300, 30), new Vector2(500, 120), eventData.text.Replace("\\n", "\n"));

        // Do we have a cancel button?
        if (eventData.cancelable)
        {
            new TextButton(new Vector2(400, 170), new Vector2(50, 20), "Cancel", delegate { onCancel(); });
        }
        // If there isn't a fail event we have a confirm button
        if(eventData.failEvent.Equals(""))
        {
            new TextButton(new Vector2(600, 170), new Vector2(50, 20), "Confirm", delegate { onConfirm(); });
        }
        // Otherwise we have pass and fail buttons
        else
        {
            new TextButton(new Vector2(500, 170), new Vector2(50, 20), "Fail", delegate { onFail(); }, Color.red);
            new TextButton(new Vector2(600, 170), new Vector2(50, 20), "Pass", delegate { onPass(); }, Color.green);
        }
    }

    // Pass and confirm are the same
    public void onPass()
    {
        onConfirm();
    }

    // Cancel cleans up
    public void onCancel()
    {
        destroy();
    }

    public void onFail()
    {
        // Destroy this dialog to close
        destroy();
        // Trigger failure event
        if (!eventData.failEvent.Equals(""))
        {
            EventHelper.triggerEvent(eventData.failEvent);
        }
    }

    public void onConfirm()
    {
        // Destroy this dialog to close
        destroy();
        // Trigger next event
        if (!eventData.nextEvent.Equals(""))
        {
            EventHelper.triggerEvent(eventData.nextEvent);
        }
    }

    public void destroy()
    {
        // Clean up everything marked as 'dialog'
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("dialog"))
            Object.Destroy(go);
    }
}