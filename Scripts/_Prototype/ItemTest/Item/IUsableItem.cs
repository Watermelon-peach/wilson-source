namespace Sample.Item
{
    //사용 가능한 아이템 (착용/ 소모)
    public interface IUsableItem
    {
        /// <summary> 아이템 사용하기(사용 성공 여부 리턴) </summary>
        bool Use();
    }

}
