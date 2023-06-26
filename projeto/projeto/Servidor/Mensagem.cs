using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servidor {
    public class Mensagem {
        public byte[] message;
        public byte[] signature;

        public Mensagem(byte[] message, byte[] signature) {
            this.message = message;
            this.signature = signature;
        }
        public Mensagem() {

        }

        public static Mensagem DeserializeMessage(byte[] mensagemBytes) {
            int mensagemLength = BitConverter.ToInt32(mensagemBytes, 0);
            byte[] mensagem = new byte[mensagemLength];
            int assinaturaLength = BitConverter.ToInt32(mensagemBytes, 4 + mensagemLength);
            byte[] assinatura = new byte[assinaturaLength];
            Array.Copy(mensagemBytes, 4 ,mensagem,0,mensagemLength);
            Array.Copy(mensagemBytes, 4+mensagemLength+4, assinatura, 0, assinaturaLength);

            Mensagem msg = new Mensagem(mensagem, assinatura);
            return msg;
        }

       public bool VerifySignature(byte[] rsaPublicKey, byte[] mensagem, byte[] assinatura) {
            ProtoIP.Crypto.RSA rsa = new ProtoIP.Crypto.RSA();
            byte[] messageHash = new ProtoIP.Crypto.SHA256(mensagem)._digest;
            return rsa.Verify(messageHash, assinatura, rsaPublicKey);

        }
    }
}
