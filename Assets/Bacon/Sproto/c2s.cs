// Generated by sprotodump. DO NOT EDIT!
// source: ./../../../tool/c2s.sproto

using System;
using Sproto;
using System.Collections.Generic;

namespace C2sSprotoType { 
	public class ball : SprotoTypeBase {
		private static int max_field_count = 14;
		
		
		private Int64 _uid; // tag 0
		public Int64 uid {
			get { return _uid; }
			set { base.has_field.set_field (0, true); _uid = value; }
		}
		public bool HasUid {
			get { return base.has_field.has_field (0); }
		}

		private Int64 _session; // tag 1
		public Int64 session {
			get { return _session; }
			set { base.has_field.set_field (1, true); _session = value; }
		}
		public bool HasSession {
			get { return base.has_field.has_field (1); }
		}

		private Int64 _radis; // tag 2
		public Int64 radis {
			get { return _radis; }
			set { base.has_field.set_field (2, true); _radis = value; }
		}
		public bool HasRadis {
			get { return base.has_field.has_field (2); }
		}

		private Int64 _length; // tag 3
		public Int64 length {
			get { return _length; }
			set { base.has_field.set_field (3, true); _length = value; }
		}
		public bool HasLength {
			get { return base.has_field.has_field (3); }
		}

		private Int64 _width; // tag 4
		public Int64 width {
			get { return _width; }
			set { base.has_field.set_field (4, true); _width = value; }
		}
		public bool HasWidth {
			get { return base.has_field.has_field (4); }
		}

		private Int64 _height; // tag 5
		public Int64 height {
			get { return _height; }
			set { base.has_field.set_field (5, true); _height = value; }
		}
		public bool HasHeight {
			get { return base.has_field.has_field (5); }
		}

		private Int64 _px; // tag 6
		public Int64 px {
			get { return _px; }
			set { base.has_field.set_field (6, true); _px = value; }
		}
		public bool HasPx {
			get { return base.has_field.has_field (6); }
		}

		private Int64 _py; // tag 7
		public Int64 py {
			get { return _py; }
			set { base.has_field.set_field (7, true); _py = value; }
		}
		public bool HasPy {
			get { return base.has_field.has_field (7); }
		}

		private Int64 _pz; // tag 8
		public Int64 pz {
			get { return _pz; }
			set { base.has_field.set_field (8, true); _pz = value; }
		}
		public bool HasPz {
			get { return base.has_field.has_field (8); }
		}

		private Int64 _dx; // tag 9
		public Int64 dx {
			get { return _dx; }
			set { base.has_field.set_field (9, true); _dx = value; }
		}
		public bool HasDx {
			get { return base.has_field.has_field (9); }
		}

		private Int64 _dy; // tag 10
		public Int64 dy {
			get { return _dy; }
			set { base.has_field.set_field (10, true); _dy = value; }
		}
		public bool HasDy {
			get { return base.has_field.has_field (10); }
		}

		private Int64 _dz; // tag 11
		public Int64 dz {
			get { return _dz; }
			set { base.has_field.set_field (11, true); _dz = value; }
		}
		public bool HasDz {
			get { return base.has_field.has_field (11); }
		}

		private Int64 _vel; // tag 12
		public Int64 vel {
			get { return _vel; }
			set { base.has_field.set_field (12, true); _vel = value; }
		}
		public bool HasVel {
			get { return base.has_field.has_field (12); }
		}

		private Int64 _ballid; // tag 13
		public Int64 ballid {
			get { return _ballid; }
			set { base.has_field.set_field (13, true); _ballid = value; }
		}
		public bool HasBallid {
			get { return base.has_field.has_field (13); }
		}

		public ball () : base(max_field_count) {}

		public ball (byte[] buffer) : base(max_field_count, buffer) {
			this.decode ();
		}

		protected override void decode () {
			int tag = -1;
			while (-1 != (tag = base.deserialize.read_tag ())) {
				switch (tag) {
				case 0:
					this.uid = base.deserialize.read_integer ();
					break;
				case 1:
					this.session = base.deserialize.read_integer ();
					break;
				case 2:
					this.radis = base.deserialize.read_integer ();
					break;
				case 3:
					this.length = base.deserialize.read_integer ();
					break;
				case 4:
					this.width = base.deserialize.read_integer ();
					break;
				case 5:
					this.height = base.deserialize.read_integer ();
					break;
				case 6:
					this.px = base.deserialize.read_integer ();
					break;
				case 7:
					this.py = base.deserialize.read_integer ();
					break;
				case 8:
					this.pz = base.deserialize.read_integer ();
					break;
				case 9:
					this.dx = base.deserialize.read_integer ();
					break;
				case 10:
					this.dy = base.deserialize.read_integer ();
					break;
				case 11:
					this.dz = base.deserialize.read_integer ();
					break;
				case 12:
					this.vel = base.deserialize.read_integer ();
					break;
				case 13:
					this.ballid = base.deserialize.read_integer ();
					break;
				default:
					base.deserialize.read_unknow_data ();
					break;
				}
			}
		}

		public override int encode (SprotoStream stream) {
			base.serialize.open (stream);

			if (base.has_field.has_field (0)) {
				base.serialize.write_integer (this.uid, 0);
			}

			if (base.has_field.has_field (1)) {
				base.serialize.write_integer (this.session, 1);
			}

			if (base.has_field.has_field (2)) {
				base.serialize.write_integer (this.radis, 2);
			}

			if (base.has_field.has_field (3)) {
				base.serialize.write_integer (this.length, 3);
			}

			if (base.has_field.has_field (4)) {
				base.serialize.write_integer (this.width, 4);
			}

			if (base.has_field.has_field (5)) {
				base.serialize.write_integer (this.height, 5);
			}

			if (base.has_field.has_field (6)) {
				base.serialize.write_integer (this.px, 6);
			}

			if (base.has_field.has_field (7)) {
				base.serialize.write_integer (this.py, 7);
			}

			if (base.has_field.has_field (8)) {
				base.serialize.write_integer (this.pz, 8);
			}

			if (base.has_field.has_field (9)) {
				base.serialize.write_integer (this.dx, 9);
			}

			if (base.has_field.has_field (10)) {
				base.serialize.write_integer (this.dy, 10);
			}

			if (base.has_field.has_field (11)) {
				base.serialize.write_integer (this.dz, 11);
			}

			if (base.has_field.has_field (12)) {
				base.serialize.write_integer (this.vel, 12);
			}

			if (base.has_field.has_field (13)) {
				base.serialize.write_integer (this.ballid, 13);
			}

			return base.serialize.close ();
		}
	}


	public class born {
	
		public class response : SprotoTypeBase {
			private static int max_field_count = 2;
			
			
			private Int64 _errorcode; // tag 0
			public Int64 errorcode {
				get { return _errorcode; }
				set { base.has_field.set_field (0, true); _errorcode = value; }
			}
			public bool HasErrorcode {
				get { return base.has_field.has_field (0); }
			}

			private ball _b; // tag 1
			public ball b {
				get { return _b; }
				set { base.has_field.set_field (1, true); _b = value; }
			}
			public bool HasB {
				get { return base.has_field.has_field (1); }
			}

			public response () : base(max_field_count) {}

			public response (byte[] buffer) : base(max_field_count, buffer) {
				this.decode ();
			}

			protected override void decode () {
				int tag = -1;
				while (-1 != (tag = base.deserialize.read_tag ())) {
					switch (tag) {
					case 0:
						this.errorcode = base.deserialize.read_integer ();
						break;
					case 1:
						this.b = base.deserialize.read_obj<ball> ();
						break;
					default:
						base.deserialize.read_unknow_data ();
						break;
					}
				}
			}

			public override int encode (SprotoStream stream) {
				base.serialize.open (stream);

				if (base.has_field.has_field (0)) {
					base.serialize.write_integer (this.errorcode, 0);
				}

				if (base.has_field.has_field (1)) {
					base.serialize.write_obj (this.b, 1);
				}

				return base.serialize.close ();
			}
		}


	}


	public class handshake {
	
		public class response : SprotoTypeBase {
			private static int max_field_count = 1;
			
			
			private Int64 _errorcode; // tag 0
			public Int64 errorcode {
				get { return _errorcode; }
				set { base.has_field.set_field (0, true); _errorcode = value; }
			}
			public bool HasErrorcode {
				get { return base.has_field.has_field (0); }
			}

			public response () : base(max_field_count) {}

			public response (byte[] buffer) : base(max_field_count, buffer) {
				this.decode ();
			}

			protected override void decode () {
				int tag = -1;
				while (-1 != (tag = base.deserialize.read_tag ())) {
					switch (tag) {
					case 0:
						this.errorcode = base.deserialize.read_integer ();
						break;
					default:
						base.deserialize.read_unknow_data ();
						break;
					}
				}
			}

			public override int encode (SprotoStream stream) {
				base.serialize.open (stream);

				if (base.has_field.has_field (0)) {
					base.serialize.write_integer (this.errorcode, 0);
				}

				return base.serialize.close ();
			}
		}


	}


	public class join {
	
		public class request : SprotoTypeBase {
			private static int max_field_count = 1;
			
			
			private Int64 _room; // tag 0
			public Int64 room {
				get { return _room; }
				set { base.has_field.set_field (0, true); _room = value; }
			}
			public bool HasRoom {
				get { return base.has_field.has_field (0); }
			}

			public request () : base(max_field_count) {}

			public request (byte[] buffer) : base(max_field_count, buffer) {
				this.decode ();
			}

			protected override void decode () {
				int tag = -1;
				while (-1 != (tag = base.deserialize.read_tag ())) {
					switch (tag) {
					case 0:
						this.room = base.deserialize.read_integer ();
						break;
					default:
						base.deserialize.read_unknow_data ();
						break;
					}
				}
			}

			public override int encode (SprotoStream stream) {
				base.serialize.open (stream);

				if (base.has_field.has_field (0)) {
					base.serialize.write_integer (this.room, 0);
				}

				return base.serialize.close ();
			}
		}


		public class response : SprotoTypeBase {
			private static int max_field_count = 4;
			
			
			private Int64 _session; // tag 0
			public Int64 session {
				get { return _session; }
				set { base.has_field.set_field (0, true); _session = value; }
			}
			public bool HasSession {
				get { return base.has_field.has_field (0); }
			}

			private string _host; // tag 1
			public string host {
				get { return _host; }
				set { base.has_field.set_field (1, true); _host = value; }
			}
			public bool HasHost {
				get { return base.has_field.has_field (1); }
			}

			private Int64 _port; // tag 2
			public Int64 port {
				get { return _port; }
				set { base.has_field.set_field (2, true); _port = value; }
			}
			public bool HasPort {
				get { return base.has_field.has_field (2); }
			}

			private List<ball> _all; // tag 3
			public List<ball> all {
				get { return _all; }
				set { base.has_field.set_field (3, true); _all = value; }
			}
			public bool HasAll {
				get { return base.has_field.has_field (3); }
			}

			public response () : base(max_field_count) {}

			public response (byte[] buffer) : base(max_field_count, buffer) {
				this.decode ();
			}

			protected override void decode () {
				int tag = -1;
				while (-1 != (tag = base.deserialize.read_tag ())) {
					switch (tag) {
					case 0:
						this.session = base.deserialize.read_integer ();
						break;
					case 1:
						this.host = base.deserialize.read_string ();
						break;
					case 2:
						this.port = base.deserialize.read_integer ();
						break;
					case 3:
						this.all = base.deserialize.read_obj_list<ball> ();
						break;
					default:
						base.deserialize.read_unknow_data ();
						break;
					}
				}
			}

			public override int encode (SprotoStream stream) {
				base.serialize.open (stream);

				if (base.has_field.has_field (0)) {
					base.serialize.write_integer (this.session, 0);
				}

				if (base.has_field.has_field (1)) {
					base.serialize.write_string (this.host, 1);
				}

				if (base.has_field.has_field (2)) {
					base.serialize.write_integer (this.port, 2);
				}

				if (base.has_field.has_field (3)) {
					base.serialize.write_obj (this.all, 3);
				}

				return base.serialize.close ();
			}
		}


	}


	public class leave {
	
		public class response : SprotoTypeBase {
			private static int max_field_count = 1;
			
			
			private Int64 _errorcode; // tag 0
			public Int64 errorcode {
				get { return _errorcode; }
				set { base.has_field.set_field (0, true); _errorcode = value; }
			}
			public bool HasErrorcode {
				get { return base.has_field.has_field (0); }
			}

			public response () : base(max_field_count) {}

			public response (byte[] buffer) : base(max_field_count, buffer) {
				this.decode ();
			}

			protected override void decode () {
				int tag = -1;
				while (-1 != (tag = base.deserialize.read_tag ())) {
					switch (tag) {
					case 0:
						this.errorcode = base.deserialize.read_integer ();
						break;
					default:
						base.deserialize.read_unknow_data ();
						break;
					}
				}
			}

			public override int encode (SprotoStream stream) {
				base.serialize.open (stream);

				if (base.has_field.has_field (0)) {
					base.serialize.write_integer (this.errorcode, 0);
				}

				return base.serialize.close ();
			}
		}


	}


	public class opcode {
	
		public class request : SprotoTypeBase {
			private static int max_field_count = 1;
			
			
			private Int64 _code; // tag 0
			public Int64 code {
				get { return _code; }
				set { base.has_field.set_field (0, true); _code = value; }
			}
			public bool HasCode {
				get { return base.has_field.has_field (0); }
			}

			public request () : base(max_field_count) {}

			public request (byte[] buffer) : base(max_field_count, buffer) {
				this.decode ();
			}

			protected override void decode () {
				int tag = -1;
				while (-1 != (tag = base.deserialize.read_tag ())) {
					switch (tag) {
					case 0:
						this.code = base.deserialize.read_integer ();
						break;
					default:
						base.deserialize.read_unknow_data ();
						break;
					}
				}
			}

			public override int encode (SprotoStream stream) {
				base.serialize.open (stream);

				if (base.has_field.has_field (0)) {
					base.serialize.write_integer (this.code, 0);
				}

				return base.serialize.close ();
			}
		}


		public class response : SprotoTypeBase {
			private static int max_field_count = 1;
			
			
			private Int64 _errorcode; // tag 0
			public Int64 errorcode {
				get { return _errorcode; }
				set { base.has_field.set_field (0, true); _errorcode = value; }
			}
			public bool HasErrorcode {
				get { return base.has_field.has_field (0); }
			}

			public response () : base(max_field_count) {}

			public response (byte[] buffer) : base(max_field_count, buffer) {
				this.decode ();
			}

			protected override void decode () {
				int tag = -1;
				while (-1 != (tag = base.deserialize.read_tag ())) {
					switch (tag) {
					case 0:
						this.errorcode = base.deserialize.read_integer ();
						break;
					default:
						base.deserialize.read_unknow_data ();
						break;
					}
				}
			}

			public override int encode (SprotoStream stream) {
				base.serialize.open (stream);

				if (base.has_field.has_field (0)) {
					base.serialize.write_integer (this.errorcode, 0);
				}

				return base.serialize.close ();
			}
		}


	}


	public class package : SprotoTypeBase {
		private static int max_field_count = 4;
		
		
		private Int64 _type; // tag 0
		public Int64 type {
			get { return _type; }
			set { base.has_field.set_field (0, true); _type = value; }
		}
		public bool HasType {
			get { return base.has_field.has_field (0); }
		}

		private Int64 _session; // tag 1
		public Int64 session {
			get { return _session; }
			set { base.has_field.set_field (1, true); _session = value; }
		}
		public bool HasSession {
			get { return base.has_field.has_field (1); }
		}

		private Int64 _index; // tag 2
		public Int64 index {
			get { return _index; }
			set { base.has_field.set_field (2, true); _index = value; }
		}
		public bool HasIndex {
			get { return base.has_field.has_field (2); }
		}

		private Int64 _version; // tag 3
		public Int64 version {
			get { return _version; }
			set { base.has_field.set_field (3, true); _version = value; }
		}
		public bool HasVersion {
			get { return base.has_field.has_field (3); }
		}

		public package () : base(max_field_count) {}

		public package (byte[] buffer) : base(max_field_count, buffer) {
			this.decode ();
		}

		protected override void decode () {
			int tag = -1;
			while (-1 != (tag = base.deserialize.read_tag ())) {
				switch (tag) {
				case 0:
					this.type = base.deserialize.read_integer ();
					break;
				case 1:
					this.session = base.deserialize.read_integer ();
					break;
				case 2:
					this.index = base.deserialize.read_integer ();
					break;
				case 3:
					this.version = base.deserialize.read_integer ();
					break;
				default:
					base.deserialize.read_unknow_data ();
					break;
				}
			}
		}

		public override int encode (SprotoStream stream) {
			base.serialize.open (stream);

			if (base.has_field.has_field (0)) {
				base.serialize.write_integer (this.type, 0);
			}

			if (base.has_field.has_field (1)) {
				base.serialize.write_integer (this.session, 1);
			}

			if (base.has_field.has_field (2)) {
				base.serialize.write_integer (this.index, 2);
			}

			if (base.has_field.has_field (3)) {
				base.serialize.write_integer (this.version, 3);
			}

			return base.serialize.close ();
		}
	}


}

