namespace Tera.StoryInjector;


public class InstructionListWrapper
{
    private List<Instruction>? _instructions = new();
    private List<Say>? _says;

    public void Set(List<Instruction> instructions)
    {
        _instructions = instructions;
        _says = null;
    }

    public void Set(List<Say> says)
    {
        _instructions = null;
        _says = says;
    }

    public void Add(SaySwitch saySwitch)
    {
        if (_instructions == null)
        {
            // TODO: Log instead!
            throw new InvalidOperationException();
        }
        _instructions.Add(saySwitch);
    }

    public void Add(Say say)
    {
        if (_instructions != null)
        {
            _instructions.Add(say);
        }
        else
        {
            _says!.Add(say);
        }
    }

    public Instruction this[int i]
    {
        get
        {
            if (_instructions != null)
            {
                return _instructions[i];
            }
            else
            {
                return _says![i];
            }
        }
    }

    public int Count
    {
        get
        {
            if (_instructions != null)
            {
                return _instructions.Count;
            }
            return _says!.Count;
        }
    }
}