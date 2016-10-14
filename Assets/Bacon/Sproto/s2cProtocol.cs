// Generated by sprotodump. DO NOT EDIT!
using System;
using Sproto;
using System.Collections.Generic;

public class S2cProtocol : ProtocolBase {
	public static  S2cProtocol Instance = new S2cProtocol();
	private S2cProtocol() {
		Protocol.SetProtocol<born> (born.Tag);
		Protocol.SetRequest<S2cSprotoType.born.request> (born.Tag);
		Protocol.SetResponse<S2cSprotoType.born.response> (born.Tag);

		Protocol.SetProtocol<handshake> (handshake.Tag);
		Protocol.SetResponse<S2cSprotoType.handshake.response> (handshake.Tag);

		Protocol.SetProtocol<leave> (leave.Tag);
		Protocol.SetRequest<S2cSprotoType.leave.request> (leave.Tag);
		Protocol.SetResponse<S2cSprotoType.leave.response> (leave.Tag);

	}

	public class born {
		public const int Tag = 2;
	}

	public class handshake {
		public const int Tag = 1;
	}

	public class leave {
		public const int Tag = 3;
	}

}