using UnityEngine;
using System.Collections;
using System;


public class FrameState {

	public float pos_x;
	public float pos_y;
	public float angle;

	public const int size = 12; // bytes
	const int numFloats = 3;

	public FrameState(float position_x, float position_y, float eulerAngle) {
		pos_x = position_x;
		pos_y = position_y;
		angle = eulerAngle;
	}

	public byte[] toBuffer() {
		float[] buf_f = new float[numFloats] {pos_x, pos_y, angle};
		byte[] buf_b = new byte[size];
		Buffer.BlockCopy(buf_f,0,buf_b,0,size);
		return buf_b;
	}

	public static FrameState fromBuffer(byte[] buf_b, int srcOffset) {
		float[] buf_f = new float[numFloats];
		Buffer.BlockCopy(buf_b,srcOffset,buf_f,0,size);
		FrameState fs = new FrameState (buf_f [0], buf_f [1], buf_f [2]);
		return fs;
	}

	public override string ToString ()
	{
		return string.Format("FrameState: p.x = " + pos_x + ", p.y = " + pos_y + ", angle = " + angle);
	}
}
