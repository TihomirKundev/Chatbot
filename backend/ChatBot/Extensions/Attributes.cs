using System;

namespace ChatBot.Extensions;


public class ScopedServiceAttribute : Attribute { }

public class SingletonServiceAttribute : Attribute { }

public class TransientServiceAttribute : Attribute { }

public class RepositoryAttribute : Attribute { }
