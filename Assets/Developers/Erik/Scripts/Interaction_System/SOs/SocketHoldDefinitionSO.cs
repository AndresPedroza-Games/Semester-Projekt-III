using UnityEngine;


[CreateAssetMenu(fileName = "SocketHoldDefinition", menuName = "Interaction/Hold Definitions/Socket")]
public class SocketHoldDefinitionSO : HoldDefinition {

	public override void Hold(Holdable holdable, HoldController holder) {
		holder.Picker.Attach(holdable);
	}


	public override void Release(Holdable holdable, HoldController holder) {
		holder.Picker.Detach(holdable);
	}

}