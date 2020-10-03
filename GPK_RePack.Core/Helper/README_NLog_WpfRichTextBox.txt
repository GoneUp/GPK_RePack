EXAMPLES AND USAGE

NOTE: Currently must be configured via code


void MainWindow_Loaded(object sender, RoutedEventArgs e)
{
    Dispatcher.Invoke(() =>
	{
		var target = new WpfRichTextBoxTarget
		{
			Name = "RichText",
			Layout =
				"[${longdate:useUTC=false}] :: [${level:uppercase=true}] :: ${logger}:${callsite} :: ${message} ${exception:innerFormat=tostring:maxInnerExceptionLevel=10:separator=,:format=tostring}",
			ControlName = LogRichTextBox.Name,
			FormName = GetType().Name,
			AutoScroll = true,
			MaxLines = 100000,
			UseDefaultRowColoringRules = true,
		};
		var asyncWrapper = new AsyncTargetWrapper { Name = "RichTextAsync", WrappedTarget = target };

		LogManager.Configuration.AddTarget(asyncWrapper.Name, asyncWrapper);
		LogManager.Configuration.LoggingRules.Insert(0, new LoggingRule("*", LogLevel.FromString(MSMQ_Settings.Default.NLog_Log_Level), asyncWrapper));
		LogManager.ReconfigExistingLoggers();

	});
}