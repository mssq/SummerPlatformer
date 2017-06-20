﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class ChooserInput : MonoBehaviour {

    private Rewired.Player rewPlayer; // The Rewired Player
    private int positionX = 0;
    private int positionY = 0;
    private int gravityState = 1; // 0 = low, 1 = med, 2 = high
    private int colorState = 0; // 0 = red, 1 = blue, 2 = green, 3 = yellow
    private bool resetJoystick = false;
    private GameObject playerOne;
    private Player player;

    private Color yellow = new Color(0.898f, 0.785f, 0.102f);
    private Color green = new Color(0.145f, 0.785f, 0.102f);
    private Color red = new Color(0.785f, 0.102f, 0.149f);
    private Color blue = new Color(0.102f, 0.141f, 0.785f);

    public Image gravityImage;
    public Image colorImage;

    private void Awake() {
        rewPlayer = ReInput.players.GetPlayer(1);
        playerOne = GameObject.FindGameObjectWithTag("Player One");
        player = playerOne.GetComponent<Player>();
    }

    void Start() {
        Rewired.Controller j = ReInput.controllers.GetController(ControllerType.Joystick, 0);
        rewPlayer.controllers.AddController(j, false);
    }

    void Update () {

        Vector2 directionalInput = new Vector2(rewPlayer.GetAxis("Move Horizontal P2"), rewPlayer.GetAxis("Move Vertical P2"));

        if (rewPlayer.GetButton("Pull Lever")) {
            interact(directionalInput);
        } else {
            if (directionalInput.x > 0.5 && positionX == 0) {
                // Move right
                positionX++;
                transform.position = new Vector2(transform.position.x + 1.4f, transform.position.y);
            } else if (directionalInput.x < -0.5 && positionX == 1) {
                // Move left
                positionX--;
                transform.position = new Vector2(transform.position.x - 1.4f, transform.position.y);
            } else if (directionalInput.y > 0.5 && positionY == 1) {
                // Move up
                positionY--;
                transform.position = new Vector2(transform.position.x, transform.position.y + 2.13f);
            } else if (directionalInput.y < -0.5 && positionY == 0) {
                // Move down
                positionY++;
                transform.position = new Vector2(transform.position.x, transform.position.y - 2.13f);
            }
        }     
    }

    private void interact(Vector2 input) {

        if (input.x < 0.2 && input.x > -0.2) {
            resetJoystick = false;
        }

        if (positionX == 0 && positionY == 0) {
            // Top left
            if (input.x > 0.5 && !resetJoystick) {
                switch (gravityState) {
                    case 0:
                        gravityImage.sprite = Resources.Load<Sprite>("Sprites/GravityIconMed");
                        player.setGravity(20);
                        gravityState++;
                        break;
                    case 1:
                        gravityImage.sprite = Resources.Load<Sprite>("Sprites/GravityIconHigh");
                        player.setGravity(40);
                        gravityState++;
                        break;
                    default:
                        break;
                }
                resetJoystick = true;

            } else if (input.x < -0.5 && !resetJoystick) {
                switch (gravityState) {
                    case 1:
                        gravityImage.sprite = Resources.Load<Sprite>("Sprites/GravityIconLow");
                        player.setGravity(5);
                        gravityState--;
                        break;
                    case 2:
                        gravityImage.sprite = Resources.Load<Sprite>("Sprites/GravityIconMed");
                        player.setGravity(20);
                        gravityState--;
                        break;
                    default:
                        break;
                }
                resetJoystick = true;
            }

        } else if (positionX == 1 && positionY == 0) {
            // Top right
            if (input.x > 0.5 && !resetJoystick) {
                switch (colorState) {
                    case 0:
                        playerOne.GetComponent<Renderer>().material.color = blue;
                        colorImage.sprite = Resources.Load<Sprite>("Sprites/PlayerColorBlue");
                        colorState++;
                        break;
                    case 1:
                        playerOne.GetComponent<Renderer>().material.color = green;
                        colorImage.sprite = Resources.Load<Sprite>("Sprites/PlayerColorGreen");
                        colorState++;
                        break;
                    case 2:
                        playerOne.GetComponent<Renderer>().material.color = yellow;
                        colorImage.sprite = Resources.Load<Sprite>("Sprites/PlayerColorYellow");
                        colorState++;
                        break;
                    default:
                        break;
                }
                resetJoystick = true;

            } else if (input.x < -0.5 && !resetJoystick) {
                switch (colorState) {
                    case 1:
                        playerOne.GetComponent<Renderer>().material.color = red;
                        colorImage.sprite = Resources.Load<Sprite>("Sprites/PlayerColorRed");
                        colorState--;
                        break;
                    case 2:
                        playerOne.GetComponent<Renderer>().material.color = blue;
                        colorImage.sprite = Resources.Load<Sprite>("Sprites/PlayerColorBlue");
                        colorState--;
                        break;
                    case 3:
                        playerOne.GetComponent<Renderer>().material.color = green;
                        colorImage.sprite = Resources.Load<Sprite>("Sprites/PlayerColorGreen");
                        colorState--;
                        break;
                    default:
                        break;
                }
                resetJoystick = true;
            }

        } else if (positionX == 0 && positionY == 1) {
            // Bottom left

        } else if (positionX == 1 && positionY == 1) {
            // Bottom right

        }
    }
}
