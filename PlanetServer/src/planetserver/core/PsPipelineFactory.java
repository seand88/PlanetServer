package planetserver.core;

import org.jboss.netty.channel.ChannelHandler;
import org.jboss.netty.channel.ChannelPipeline;
import org.jboss.netty.channel.ChannelPipelineFactory;
import org.jboss.netty.channel.DefaultChannelPipeline;
import org.jboss.netty.handler.codec.compression.ZlibDecoder;
import org.jboss.netty.handler.codec.compression.ZlibEncoder;
import org.jboss.netty.handler.codec.frame.DelimiterBasedFrameDecoder;
import org.jboss.netty.handler.codec.frame.Delimiters;
import org.jboss.netty.handler.codec.string.StringDecoder;
import org.jboss.netty.handler.codec.string.StringEncoder;
import org.jboss.netty.handler.execution.ExecutionHandler;
import org.jboss.netty.util.CharsetUtil;

public class PsPipelineFactory implements ChannelPipelineFactory
{
    private final ChannelHandler handler;
    private final ExecutionHandler execHandler;

    public PsPipelineFactory(ChannelHandler handler, ExecutionHandler execHandler)
    {
        this.handler = handler;
        this.execHandler = execHandler;
    }

    @Override
    public ChannelPipeline getPipeline() throws Exception
    {
        int maxMessageLength = 1048576;//1 mb
        ChannelPipeline pipeline = new DefaultChannelPipeline();

        // Add the text line codec combination first,
        // pipeline.addLast("zlibDecoder", new ZlibDecoder());
        // pipeline.addLast("zlibEncoder", new ZlibEncoder());
        pipeline.addLast("frameDecoder", new DelimiterBasedFrameDecoder(maxMessageLength, Delimiters.nulDelimiter()));
        pipeline.addLast("decoder", new StringDecoder(CharsetUtil.UTF_8));
        pipeline.addLast("encoder", new StringEncoder(CharsetUtil.UTF_8));
        //add the new execution handler
        pipeline.addLast("executor", execHandler);
        // and then business logic.
        pipeline.addLast("handler", handler);

        return pipeline;
    }
}
