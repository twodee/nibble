using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour {
  public TextAsset level;
  public GameObject doorPrefab;
  public GameObject whiteBlockPrefab;
  public GameObject blackBlockPrefab;
  public GameObject whitePlayer;
  public GameObject blackPlayer;
  public GameObject selectionFrame;
  public TextMeshProUGUI operationLabel;
  public new Camera camera;

  private Transform prefabParent;
  private int width;
  private int height;
  private int[,] grid;
  private int subject;
  private int term;
  private int digitCount;
  private string rator;
  private bool isSelectionLocked;

  void Start() {
    digitCount = 0;
    term = 0;
    rator = null;
    isSelectionLocked = false;

    prefabParent = GameObject.Find("Prefab Parent").transform;

    string levelText = level.text.Trim();
    string[] lines = Regex.Split(levelText, "\r\n|\r|\n");

    height = lines.Length;
    width = lines[0].Length;
    grid = new int[height, width];

    for (int r = 0; r < height; ++r) {
      for (int c = 0; c < width; ++c) {
        Vector2 position = new Vector2(c, lines.Length - 1 - r);
        char symbol = lines[r][c];

        GameObject block;
        if (symbol == '0' || symbol == 'w' || symbol == 'W') {
          block = Instantiate(blackBlockPrefab);
          grid[r, c] = 0;
        } else {
          block = Instantiate(whiteBlockPrefab);
          grid[r, c] = 1;
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

    camera.transform.position = new Vector3((width - 1) * 0.5f, (height - 1) * 0.5f, camera.transform.position.z);
  }

  void Update() {
    if (!isSelectionLocked) {
      Vector3 mouseScreen = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
      Vector3 mouseWorld = camera.ScreenToWorldPoint(mouseScreen);
      int c = Mathf.Clamp(Mathf.RoundToInt(mouseWorld.x) - 2, 0, width - 4);
      int r = Mathf.Clamp(Mathf.RoundToInt(mouseWorld.y), 0, height - 1);
      selectionFrame.transform.position = new Vector3(c, r, selectionFrame.transform.position.z);

      r = height - 1 - r;
      if (Input.GetMouseButtonDown(0)) {
        subject = (grid[r, c + 0] << 3) +
                  (grid[r, c + 1] << 2) +
                  (grid[r, c + 2] << 1) +
                  (grid[r, c + 3] << 0);
        isSelectionLocked = true;
      }
    }

    if (Input.GetKeyDown(KeyCode.BackQuote)) {
      rator = "~";
    } else if (Input.GetKeyDown(KeyCode.Ampersand) || (Input.GetKeyDown(KeyCode.Alpha7) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))) {
      rator = "&";
    } else if (Input.GetKeyDown(KeyCode.Pipe) || (Input.GetKeyDown(KeyCode.Backslash) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))) {
      rator = "|";
    } else if (Input.GetKeyDown(KeyCode.Less) || (Input.GetKeyDown(KeyCode.Comma) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))) {
      rator = "<<";
    } else if (Input.GetKeyDown(KeyCode.Greater) || (Input.GetKeyDown(KeyCode.Period) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))) {
      rator = ">>";
    } else if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)) {
      term = (term << 1) | 0;
      digitCount += 1;
    } else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) {
      term = (term << 1) | 1;
      digitCount += 1;
    }

    if (rator != null) {
      if (rator == "~") {
        int result = Evaluate();
        operationLabel.text = string.Format("~{0} = {1}", Quadify(subject), Quadify(result));
      } else if (digitCount == 4) {
        int result = Evaluate();
        operationLabel.text = string.Format("{0} {1} {2} = {3}", Quadify(subject), rator, Quadify(term), Quadify(result));
      } else if (digitCount == 0) {
        operationLabel.text = string.Format("{0} {1} ????", Quadify(subject), rator);
      } else {
        operationLabel.text = string.Format("{0} {1} {2}", Quadify(subject), rator, System.Convert.ToString(term, 2).PadLeft(digitCount, '0').PadRight(4, '?'));
      }
    } else {
      operationLabel.text = string.Format("{0}", Quadify(subject));
    }
  }

  private int Evaluate() {
    if (rator == "~") {
      return ~subject & 0b1111;
    } else if (rator == "&") {
      return subject & term;
    } else if (rator == "|") {
      return subject | term;
    } else if (rator == "^") {
      return subject ^ term;
    } else if (rator == "<<") {
      return (subject << term) & 0b1111;
    } else if (rator == ">>") {
      return (subject >> term) & 0b1111;
    } else {
      throw new System.Exception("unknown rator");
    }
  }

  static string Quadify(int x) {
    return System.Convert.ToString(x, 2).PadLeft(4, '0');
  }
}
