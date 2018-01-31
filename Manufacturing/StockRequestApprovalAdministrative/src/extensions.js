const extensions = {
    registerExtensions: function () {
        String.prototype.trimEnd = function (charlist) {
            var index = this.lastIndexOf(charlist);
            if (index == -1) return this;
            var str = this.substring(0, index);
            return str;
        };
        Array.prototype.contains = function (obj) {
            var i = this.length;
            while (i--) {
                if (this[i] == obj) {
                    return true;
                }
            }
            return false;
        };
        Array.prototype.equals = function (array) {
            // if the other array is a falsy value, return
            if (!array) return false;

            // compare lengths - can save a lot of time
            if (this.length != array.length) return false;

            for (var i = 0, l = this.length; i < l; i++) {
                // Check if we have nested arrays
                if (this[i] instanceof Array && array[i] instanceof Array) {
                    // recurse into the nested arrays
                    if (!this[i].equals(array[i])) return false;
                } else if (this[i] != array[i]) {
                    // Warning - two different object instances will never be equal: {x:20} != {x:20}
                    return false;
                }
            }
            return true;
        };
        // Hide method from for-in loops
        Object.defineProperty(Array.prototype, "equals", { enumerable: false });
    }
}
export default extensions