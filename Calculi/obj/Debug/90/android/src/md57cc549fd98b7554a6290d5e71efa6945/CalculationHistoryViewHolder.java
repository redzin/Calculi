package md57cc549fd98b7554a6290d5e71efa6945;


public class CalculationHistoryViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Calculi.CalculationHistoryViewHolder, Calculi", CalculationHistoryViewHolder.class, __md_methods);
	}


	public CalculationHistoryViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == CalculationHistoryViewHolder.class)
			mono.android.TypeManager.Activate ("Calculi.CalculationHistoryViewHolder, Calculi", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
