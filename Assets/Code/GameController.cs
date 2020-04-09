using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
/* using System.IO.Ports; */

public class GameController : MonoBehaviour {
  public TextAsset level;
  public GameObject doorPrefab;
  public GameObject whiteBlockPrefab;
  public GameObject blackBlockPrefab;
  public GameObject whitePlayer;
  public GameObject blackPlayer;
  public GameObject selectionFrame;
  public new Camera camera;

  private Transform prefabParent;
  private int width;
  private int height;

  void Start() {
    prefabParent = GameObject.Find("Prefab Parent").transform;

    string levelText = level.text.Trim();
    string[] lines = Regex.Split(levelText, "\r\n|\r|\n");
    for (int r = 0; r < lines.Length; ++r) {
      for (int c = 0; c < lines[r].Length; ++c) {
        Vector2 position = new Vector2(c, lines.Length - 1 - r);
        char symbol = lines[r][c];

        GameObject block;
        if (symbol == '0' || symbol == 'w' || symbol == 'W') {
          block = Instantiate(blackBlockPrefab);
        } else {
          block = Instantiate(whiteBlockPrefab);
        }
        block.transform.SetParent(prefabParent);
        block.transform.position = position;

        if (symbol == 'W' || symbol == 'B') {
          GameObject door = Instantiate(doorPrefab);
          door.transform.SetParent(prefabParent);
          door.transform.position = new Vector3(position.x, position.y, -2);
        }

        if (symbol == 'w') {
          whitePlayer.transform.position = new Vector3(position.x, position.y, -1);
        } else if (symbol == 'b') {
          blackPlayer.transform.position = new Vector3(position.x, position.y, -1);
        }
      }
    }

    height = lines.Length;
    width = lines[0].Length;
    camera.transform.position = new Vector3((width - 1) * 0.5f, (height - 1) * 0.5f, camera.transform.position.z);
  }

  void Update() {
    Vector3 mouseScreen = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
    Vector3 mouseWorld = camera.ScreenToWorldPoint(mouseScreen);
    int c = Mathf.Clamp(Mathf.RoundToInt(mouseWorld.x) - 2, 0, width - 4);
    int r = Mathf.Clamp(Mathf.RoundToInt(mouseWorld.y), 0, height - 1);
    selectionFrame.transform.position = new Vector3(c, r, selectionFrame.transform.position.z);
  }
}
