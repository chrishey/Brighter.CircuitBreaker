using paramore.brighter.commandprocessor;

namespace Brighter.CircuitBreaker.Service
{
    public class Processor
    {
	    private readonly IAmACommandProcessor _commandProcessor;
	    private readonly IAmACommandProcessorBuilder _commandProcessorBuilder;

	    public Processor(IAmACommandProcessor commandProcessor, IAmACommandProcessorBuilder commandProcessorBuilder)
	    {
		    _commandProcessor = commandProcessor;
		    _commandProcessorBuilder = commandProcessorBuilder;
	    }

	    public void Process()
	    {
		    
	    }
    }
}
