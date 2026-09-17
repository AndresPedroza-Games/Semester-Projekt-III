using UnityEngine;


[CreateAssetMenu(fileName = "PieceData", menuName = "1st Puzzle/PieceData")]
public class PieceData : ScriptableObject {
	[field: SerializeField] public Vector2Int CorrectPosition { get; private set; }
	[field: SerializeField] public int CorrectRotationStep { get; private set; }

}