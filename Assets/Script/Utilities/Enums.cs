public enum ItemType
{

    Seed,Commodity,Furniture,//���ӣ���Ʒ���Ҿ�
    //��ͷ����ͷ�����鹤�ߣ��ո�ߣ���ˮ���ռ���
    HoeTool,ChopTool,BreakTool,ReapTool,WaterTool,CollectTool,
    ReapableScenery//�Ӳ�
}

//����
public enum SlotType
{
    Bag,Box,Shop
}


//��Ʒ����λ��
public enum InventoryLocation
{
    //��ұ�������������
    Player,Box
}

//��Ʒ��ʹ��
public enum PartType
{
    None,Carry,Hoe,Break,Water,Collect,Chop,Reap
}
//��Ҳ���
public enum PartName
{
    Body,Hair,Arm,Tool
}
//����
public enum Season
{
    ����,�ļ�,�＾,����
}

//��Ƭ������
public enum GridType
{
    //�ڣ�����Ʒ�� ��npc�ϰ�
    Diggable,DropItem,PlaceFurniture,NpcObstacle
}

//����Ч��
public enum ParticleEffectType
{
    None,LeavesFalling01,LeavesFalling02,Rock/*ʯͷ*/,ReapableScenery/*������*/
}