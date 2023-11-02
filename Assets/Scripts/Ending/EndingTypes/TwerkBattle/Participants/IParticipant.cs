
using System;

public interface IParticipant
{
    public event Action TurnEnded;
    public void GiveEnemy(Participant participant);
}
