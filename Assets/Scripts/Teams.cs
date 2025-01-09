using UnityEngine;

[CreateAssetMenu(fileName = "NewTeam", menuName = "New Team")]
public class Teams : ScriptableObject
{
    public Sprite teamImage;
    public string teamName;
    public string teamDescription;

    public int velocidad;

    public Color colort;
    public int habilidadEnfriamiento;

    public int vida;
}
