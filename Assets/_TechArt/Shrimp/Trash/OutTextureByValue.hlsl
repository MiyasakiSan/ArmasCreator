void outTextureByFloat_float(float4 T1, float4 T2, float4 T3, float4 T4, float Value, out float4 OUT_T)
{
	if (Value >= 0 && Value <= 0.25f)
	{
		OUT_T = T1;

	}

	else if (Value > 0.25f && Value <= 0.5f)
	{
		OUT_T = T2;
	}
	
	else if (Value > 0.5f && Value <= 0.75f)
	{
		OUT_T = T3;
	}
	
	else if (Value > 0.75f && Value <= 1)
	{
		OUT_T = T4;
	}
}