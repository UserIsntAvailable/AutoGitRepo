using System;


namespace AutoGitRepo {

    class ArgumentNameNotUsedException : Exception {

        public ArgumentNameNotUsedException() {
        }

        public ArgumentNameNotUsedException(string message) : base(message) {
        }
    }

    class ArgumentNotImplementedException : Exception {

        public ArgumentNotImplementedException() {
        }

        public ArgumentNotImplementedException(string message) : base(message) {
        }
    }
}
