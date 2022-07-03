namespace Nothing;

public class LoveYou : ILoveYou {
    private const string love = "I Love You";

    static LoveYou() => WriteLine(love);

    public void SayLove() => WriteLine(love);
}