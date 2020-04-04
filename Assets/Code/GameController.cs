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
  public new Camera camera;

  private Transform prefabParent;

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

    camera.transform.position = new Vector3((lines[0].Length - 1) * 0.5f, (lines.Length - 1) * 0.5f, camera.transform.position.z);
  }
}
