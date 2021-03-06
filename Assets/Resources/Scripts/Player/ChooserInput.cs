﻿using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class ChooserInput : PlayerManager {

    private int positionX = 0;
    private int positionY = 0;
    private bool resetJoystick = false;
    private bool resetKeyboard = false;
    private int gravityState = 1; // 0 = low, 1 = med, 2 = high
    private int colorState = 0; // 0 = red, 1 = blue, 2 = green, 3 = yellow

    private SpriteRenderer chooserSprite;
    private SpriteRenderer rArrowSprite;
    private SpriteRenderer lArrowSprite;
    private AudioSource[] sounds;
    private AudioSource movePlusSound;
    private AudioSource moveMinusSound;
    private AudioSource moveSound;
    public GameObject rArrow;
    public GameObject lArrow;

    private Animator animJoystick;
    public GameObject arcadeJoystick;

    public Image gravityImage;
    public Image colorImage;
    

    protected override void Awake() {
        base.Awake();

        rewPlayer = ReInput.players.GetPlayer(1);
        chooserSprite = GetComponent<SpriteRenderer>();
        rArrowSprite = rArrow.GetComponent<SpriteRenderer>();
        lArrowSprite = lArrow.GetComponent<SpriteRenderer>();
        animJoystick = arcadeJoystick.GetComponent<Animator>();
        sounds = GetComponents<AudioSource>();
        movePlusSound = sounds[0];
        moveMinusSound = sounds[1];
        moveSound = sounds[2];
    }

    void Start() {
        Rewired.Controller j = ReInput.controllers.GetController(ControllerType.Joystick, 0);
        rewPlayer.controllers.AddController(j, false);
    }

    void Update () {

        Vector2 directionalInput = new Vector2(rewPlayer.GetAxis("Move Horizontal P2"), rewPlayer.GetAxis("Move Vertical P2"));

        if (rewPlayer.GetButtonUp("Interact Right") || rewPlayer.GetButtonUp("Interact Left")) {
            resetKeyboard = false;
            animJoystick.SetInteger("Position", 0);
        }

        if (rewPlayer.GetButton("Pull Lever")) {
            chooserSprite.enabled = false;
            interact(directionalInput);
        } else {
            if (directionalInput.x > 0.25 && positionX == 0 || rewPlayer.GetButton("Move Right") && positionX == 0) {
                // Move right
                positionX++;
                transform.position = new Vector2(transform.position.x + 1.358f, transform.position.y);
                lArrow.transform.position = new Vector2(lArrow.transform.position.x + 1.385f, lArrow.transform.position.y);
                rArrow.transform.position = new Vector2(rArrow.transform.position.x + 1.385f, rArrow.transform.position.y);
                animJoystick.SetInteger("Position", 1);
                playSound(2);
            } else if (directionalInput.x < -0.25 && positionX == 1 || rewPlayer.GetButton("Move Left") && positionX == 1) {
                // Move left
                positionX--;
                transform.position = new Vector2(transform.position.x - 1.358f, transform.position.y);
                lArrow.transform.position = new Vector2(lArrow.transform.position.x - 1.385f, lArrow.transform.position.y);
                rArrow.transform.position = new Vector2(rArrow.transform.position.x - 1.385f, rArrow.transform.position.y);
                animJoystick.SetInteger("Position", 2);
                playSound(2);
            } else if (directionalInput.y > 0.25 && positionY == 1 || rewPlayer.GetButton("Move Up") && positionY == 1) {
                // Move up
                positionY--;
                transform.position = new Vector2(transform.position.x, transform.position.y + 2.13f);
                lArrow.transform.position = new Vector2(lArrow.transform.position.x, lArrow.transform.position.y + 2.13f);
                rArrow.transform.position = new Vector2(rArrow.transform.position.x, rArrow.transform.position.y + 2.13f);
                playSound(2);
            } else if (directionalInput.y < -0.25 && positionY == 0 || rewPlayer.GetButton("Move Down") && positionY == 0) {
                // Move down
                positionY++;
                transform.position = new Vector2(transform.position.x, transform.position.y - 2.13f);
                lArrow.transform.position = new Vector2(lArrow.transform.position.x, lArrow.transform.position.y - 2.13f);
                rArrow.transform.position = new Vector2(rArrow.transform.position.x, rArrow.transform.position.y - 2.13f);
                playSound(2);
            } else if (directionalInput.x < 0.2 && directionalInput.x > - 0.2) {
                animJoystick.SetInteger("Position", 0);
            }
        }

        if (rewPlayer.GetButtonUp("Pull Lever")) {
            chooserSprite.enabled = true;
            lArrowSprite.enabled = false;
            rArrowSprite.enabled = false;
        }
    }

    private void playSound(int sound) {
        if (sound == 0) {
            if (!moveMinusSound.isPlaying)
                moveMinusSound.Play();
        } else if (sound == 1) {
            if (!movePlusSound.isPlaying)
                movePlusSound.Play();
        } else if (sound == 2) {
            if (!moveSound.isPlaying)
                moveSound.Play();
        }
    }

    // When player presses button, interact with arcade machine and change player ones values
    private void interact(Vector2 input) {

        showArrow();

        if (input.x < 0.25 && input.x > -0.25) {
            resetJoystick = false;
            animJoystick.SetInteger("Position", 0);
        }

        if (positionX == 0 && positionY == 0) {
            // Top left
            if (input.x > 0.25 && !resetJoystick || rewPlayer.GetButton("Interact Right") && !resetKeyboard) {
                animJoystick.SetInteger("Position", 1);

                switch (gravityState) {
                    case 0:
                        gravityImage.sprite = Resources.Load<Sprite>("Sprites/GravityIconMed");
                        playerScript.SetGravity(5.5f);
                        gravityState++;
                        playSound(1);
                        break;
                    case 1:
                        gravityImage.sprite = Resources.Load<Sprite>("Sprites/GravityIconHigh");
                        playerScript.SetGravity(8);
                        gravityState++;
                        playSound(1);
                        break;
                    default:
                        break;
                }
                resetJoystick = true;
                resetKeyboard = true;

            } else if (input.x < -0.25 && !resetJoystick || rewPlayer.GetButton("Interact Left") && !resetKeyboard) {
                animJoystick.SetInteger("Position", 2);

                switch (gravityState) {
                    case 1:
                        gravityImage.sprite = Resources.Load<Sprite>("Sprites/GravityIconLow");
                        playerScript.SetGravity(3);
                        gravityState--;
                        playSound(0);
                        break;
                    case 2:
                        gravityImage.sprite = Resources.Load<Sprite>("Sprites/GravityIconMed");
                        playerScript.SetGravity(5.5f);
                        gravityState--;
                        playSound(0);
                        break;
                    default:
                        break;
                }
                resetJoystick = true;
                resetKeyboard = true;
            }

        } else if (positionX == 1 && positionY == 0) {
            // Top right
            if (input.x > 0.25 && !resetJoystick || rewPlayer.GetButton("Interact Right") && !resetKeyboard) {
                animJoystick.SetInteger("Position", 1);

                switch (colorState) {
                    case 0:
                        sprite.color = blue;
                        colorImage.sprite = Resources.Load<Sprite>("Sprites/PlayerColorBlue");
                        colorState++;
                        playSound(1);
                        break;
                    case 1:
                        sprite.color = green;
                        colorImage.sprite = Resources.Load<Sprite>("Sprites/PlayerColorGreen");
                        colorState++;
                        playSound(1);
                        break;
                    case 2:
                        sprite.color = yellow;
                        colorImage.sprite = Resources.Load<Sprite>("Sprites/PlayerColorYellow");
                        colorState++;
                        playSound(1);
                        break;
                    default:
                        break;
                }
                resetJoystick = true;
                resetKeyboard = true;

            } else if (input.x < -0.25 && !resetJoystick || rewPlayer.GetButton("Interact Left") && !resetKeyboard) {
                animJoystick.SetInteger("Position", 2);

                switch (colorState) {
                    case 1:
                        sprite.color = red;
                        colorImage.sprite = Resources.Load<Sprite>("Sprites/PlayerColorRed");
                        colorState--;
                        playSound(0);
                        break;
                    case 2:
                        sprite.color = blue;
                        colorImage.sprite = Resources.Load<Sprite>("Sprites/PlayerColorBlue");
                        colorState--;
                        playSound(0);
                        break;
                    case 3:
                        sprite.color = green;
                        colorImage.sprite = Resources.Load<Sprite>("Sprites/PlayerColorGreen");
                        colorState--;
                        playSound(0);
                        break;
                    default:
                        break;
                }
                resetJoystick = true;
                resetKeyboard = true;
            }

        } else if (positionX == 0 && positionY == 1) {
            // Bottom left

        } else if (positionX == 1 && positionY == 1) {
            // Bottom right

        }
    }

    private void showArrow() {
        if (positionX == 0 && positionY == 0) {
            switch (gravityState) {
                case 0:
                    lArrowSprite.enabled = false;
                    rArrowSprite.enabled = true;
                    break;
                case 1:
                    lArrowSprite.enabled = true;
                    rArrowSprite.enabled = true;
                    break;
                case 2:
                    lArrowSprite.enabled = true;
                    rArrowSprite.enabled = false;
                    break;
            }
        } else if (positionX == 1 && positionY == 0) {
            switch (colorState) {
                case 0:
                    lArrowSprite.enabled = false;
                    rArrowSprite.enabled = true;
                    break;
                case 1:
                    lArrowSprite.enabled = true;
                    rArrowSprite.enabled = true;
                    break;
                case 2:
                    lArrowSprite.enabled = true;
                    rArrowSprite.enabled = true;
                    break;
                case 3:
                    lArrowSprite.enabled = true;
                    rArrowSprite.enabled = false;
                    break;
            }
        }
        
    }

    public int getColorState() {
        return colorState;
    }

    public void setColorState(int colorState) {
        this.colorState = colorState;
    }

    public int getGravityState() {
        return gravityState;
    }

    public void setGravityState(int gravityState) {
        this.gravityState = gravityState;
    }
}
